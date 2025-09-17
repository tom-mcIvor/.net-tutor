using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using DotNetTutor.Api.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DotNetTutor.Api.Services;

public class CognitoService
{
  private readonly IAmazonCognitoIdentityProvider _cognitoClient;
  private readonly IConfiguration _configuration;
  private readonly ILogger<CognitoService> _logger;
  private readonly JwtService _jwtService;

  public CognitoService(
      IAmazonCognitoIdentityProvider cognitoClient,
      IConfiguration configuration,
      ILogger<CognitoService> logger,
      JwtService jwtService)
  {
    _cognitoClient = cognitoClient;
    _configuration = configuration;
    _logger = logger;
    _jwtService = jwtService;
  }

  private string CalculateSecretHash(string username)
  {
    var clientSecret = _configuration["AWS:Cognito:ClientSecret"];
    var clientId = _configuration["AWS:Cognito:ClientId"];

    if (string.IsNullOrEmpty(clientSecret))
      return null;

    var message = username + clientId;
    var keyBytes = Encoding.UTF8.GetBytes(clientSecret);
    var messageBytes = Encoding.UTF8.GetBytes(message);

    using (var hmac = new HMACSHA256(keyBytes))
    {
      var hashBytes = hmac.ComputeHash(messageBytes);
      return Convert.ToBase64String(hashBytes);
    }
  }

  public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
  {
    try
    {
      // Log configuration validation
      var clientId = _configuration["AWS:Cognito:ClientId"];
      var clientSecret = _configuration["AWS:Cognito:ClientSecret"];
      var userPoolId = _configuration["AWS:Cognito:UserPoolId"];
      var region = _configuration["AWS:Region"];
      
      _logger.LogInformation("AWS Cognito Configuration Check:");
      _logger.LogInformation("- ClientId: {ClientId}", string.IsNullOrEmpty(clientId) ? "MISSING" : (clientId.StartsWith("your-") ? "PLACEHOLDER" : "SET"));
      _logger.LogInformation("- ClientSecret: {ClientSecret}", string.IsNullOrEmpty(clientSecret) ? "MISSING" : (clientSecret.StartsWith("your-") ? "PLACEHOLDER" : "SET"));
      _logger.LogInformation("- UserPoolId: {UserPoolId}", string.IsNullOrEmpty(userPoolId) ? "MISSING" : (userPoolId.StartsWith("your-") ? "PLACEHOLDER" : "SET"));
      _logger.LogInformation("- Region: {Region}", string.IsNullOrEmpty(region) ? "MISSING" : region);
      
      var signUpRequest = new SignUpRequest
      {
        ClientId = clientId,
        Username = registerDto.Email,
        Password = registerDto.Password,
        UserAttributes = new List<AttributeType>
                {
                    new() { Name = "email", Value = registerDto.Email },
                    new() { Name = "name", Value = $"{registerDto.FirstName} {registerDto.LastName}".Trim() }
                }
      };

      var secretHash = CalculateSecretHash(registerDto.Email);
      if (!string.IsNullOrEmpty(secretHash))
      {
        signUpRequest.SecretHash = secretHash;
        _logger.LogInformation("Secret hash calculated and added to request");
      }
      else
      {
        _logger.LogWarning("No secret hash calculated - ClientSecret may be missing");
      }

      _logger.LogInformation("Attempting Cognito SignUp for user: {Email}", registerDto.Email);
      var response = await _cognitoClient.SignUpAsync(signUpRequest);
      _logger.LogInformation("Cognito SignUp successful for user: {Email}, UserConfirmed: {UserConfirmed}", registerDto.Email, response.UserConfirmed);

      return new AuthResponseDto
      {
        Email = registerDto.Email,
        FirstName = registerDto.FirstName,
        LastName = registerDto.LastName,
        RequiresVerification = !response.UserConfirmed
      };
    }
    catch (Amazon.CognitoIdentityProvider.Model.InvalidParameterException ex)
    {
      _logger.LogError(ex, "Invalid parameter error registering user {Email}: {Message}", registerDto.Email, ex.Message);
      throw;
    }
    catch (Amazon.CognitoIdentityProvider.Model.ResourceNotFoundException ex)
    {
      _logger.LogError(ex, "AWS Cognito resource not found for user {Email}: {Message} - Check UserPoolId and ClientId configuration", registerDto.Email, ex.Message);
      throw;
    }
    catch (Amazon.CognitoIdentityProvider.Model.NotAuthorizedException ex)
    {
      _logger.LogError(ex, "AWS Cognito authorization error for user {Email}: {Message} - Check ClientSecret and permissions", registerDto.Email, ex.Message);
      throw;
    }
    catch (Amazon.CognitoIdentityProvider.AmazonCognitoIdentityProviderException ex)
    {
      _logger.LogError(ex, "AWS Cognito service error registering user {Email}: {Message}", registerDto.Email, ex.Message);
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Unexpected error registering user {Email}: {Message}", registerDto.Email, ex.Message);
      throw;
    }
  }

  public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
  {
    try
    {
      var authRequest = new InitiateAuthRequest
      {
        ClientId = _configuration["AWS:Cognito:ClientId"],
        AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
        AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", loginDto.Email },
                    { "PASSWORD", loginDto.Password }
                }
      };

      var secretHash = CalculateSecretHash(loginDto.Email);
      if (!string.IsNullOrEmpty(secretHash))
      {
        authRequest.AuthParameters.Add("SECRET_HASH", secretHash);
      }

      var response = await _cognitoClient.InitiateAuthAsync(authRequest);

      if (response.AuthenticationResult != null)
      {
        // Get user attributes
        var userRequest = new GetUserRequest
        {
          AccessToken = response.AuthenticationResult.AccessToken
        };

        var userResponse = await _cognitoClient.GetUserAsync(userRequest);

        var fullName = userResponse.UserAttributes.FirstOrDefault(x => x.Name == "name")?.Value ?? "";
        var email = userResponse.UserAttributes.FirstOrDefault(x => x.Name == "email")?.Value;

        // Split full name into first and last name
        var nameParts = fullName.Split(' ', 2);
        var firstName = nameParts.Length > 0 ? nameParts[0] : "";
        var lastName = nameParts.Length > 1 ? nameParts[1] : "";

        return new AuthResponseDto
        {
          Token = response.AuthenticationResult.AccessToken,
          RefreshToken = response.AuthenticationResult.RefreshToken,
          Email = email ?? loginDto.Email,
          FirstName = firstName,
          LastName = lastName,
          ExpiresAt = DateTime.UtcNow.AddSeconds(response.AuthenticationResult.ExpiresIn)
        };
      }

      throw new UnauthorizedAccessException("Authentication failed");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error logging in user {Email}", loginDto.Email);
      throw;
    }
  }

  public async Task ConfirmSignUpAsync(ConfirmSignUpDto confirmDto)
  {
    try
    {
      var request = new ConfirmSignUpRequest
      {
        ClientId = _configuration["AWS:Cognito:ClientId"],
        Username = confirmDto.Email,
        ConfirmationCode = confirmDto.ConfirmationCode
      };

      var secretHash = CalculateSecretHash(confirmDto.Email);
      if (!string.IsNullOrEmpty(secretHash))
      {
        request.SecretHash = secretHash;
      }

      await _cognitoClient.ConfirmSignUpAsync(request);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error confirming sign up for user {Email}", confirmDto.Email);
      throw;
    }
  }

  public async Task ResendConfirmationCodeAsync(ResendConfirmationDto resendDto)
  {
    try
    {
      var request = new ResendConfirmationCodeRequest
      {
        ClientId = _configuration["AWS:Cognito:ClientId"],
        Username = resendDto.Email
      };

      var secretHash = CalculateSecretHash(resendDto.Email);
      if (!string.IsNullOrEmpty(secretHash))
      {
        request.SecretHash = secretHash;
      }

      await _cognitoClient.ResendConfirmationCodeAsync(request);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error resending confirmation code for user {Email}", resendDto.Email);
      throw;
    }
  }

  public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
  {
    try
    {
      var request = new ForgotPasswordRequest
      {
        ClientId = _configuration["AWS:Cognito:ClientId"],
        Username = forgotPasswordDto.Email
      };

      await _cognitoClient.ForgotPasswordAsync(request);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error initiating forgot password for user {Email}", forgotPasswordDto.Email);
      throw;
    }
  }

  public async Task ConfirmForgotPasswordAsync(ResetPasswordDto resetPasswordDto)
  {
    try
    {
      var request = new ConfirmForgotPasswordRequest
      {
        ClientId = _configuration["AWS:Cognito:ClientId"],
        Username = resetPasswordDto.Email,
        ConfirmationCode = resetPasswordDto.ConfirmationCode,
        Password = resetPasswordDto.NewPassword
      };

      await _cognitoClient.ConfirmForgotPasswordAsync(request);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error confirming forgot password for user {Email}", resetPasswordDto.Email);
      throw;
    }
  }

  public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
  {
    try
    {
      var request = new InitiateAuthRequest
      {
        ClientId = _configuration["AWS:Cognito:ClientId"],
        AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
        AuthParameters = new Dictionary<string, string>
                {
                    { "REFRESH_TOKEN", refreshToken }
                }
      };

      var response = await _cognitoClient.InitiateAuthAsync(request);

      if (response.AuthenticationResult != null)
      {
        return new AuthResponseDto
        {
          Token = response.AuthenticationResult.AccessToken,
          RefreshToken = response.AuthenticationResult.RefreshToken ?? refreshToken,
          ExpiresAt = DateTime.UtcNow.AddSeconds(response.AuthenticationResult.ExpiresIn)
        };
      }

      throw new UnauthorizedAccessException("Token refresh failed");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error refreshing token");
      throw;
    }
  }

  public async Task<AuthResponseDto> HandleGoogleOAuthAsync(string authorizationCode, string redirectUri)
  {
    try
    {
      _logger.LogInformation("=== GOOGLE OAUTH DEBUG START ===");
      _logger.LogInformation("Starting Google OAuth flow - AuthCode: {AuthCodeLength} chars, RedirectUri: {RedirectUri}",
        authorizationCode?.Length ?? 0, redirectUri);
      
      // Validate input parameters
      if (string.IsNullOrEmpty(authorizationCode))
      {
        _logger.LogError("VALIDATION ERROR: Authorization code is null or empty");
        throw new ArgumentException("Authorization code is required");
      }
      
      if (string.IsNullOrEmpty(redirectUri))
      {
        _logger.LogError("VALIDATION ERROR: Redirect URI is null or empty");
        throw new ArgumentException("Redirect URI is required");
      }
      
      _logger.LogInformation("Input validation passed - proceeding with token exchange");
      
      // Exchange authorization code for tokens with Google
      var tokenResponse = await ExchangeGoogleAuthCodeAsync(authorizationCode, redirectUri);
      
      _logger.LogInformation("Successfully exchanged auth code for tokens - Email: {Email}", tokenResponse.Email);
      
      // Use the ID token to authenticate with Cognito
      var authRequest = new InitiateAuthRequest
      {
        ClientId = _configuration["AWS:Cognito:ClientId"],
        AuthFlow = AuthFlowType.USER_PASSWORD_AUTH, // This will be changed to support federated auth
        AuthParameters = new Dictionary<string, string>
        {
          { "USERNAME", tokenResponse.Email },
          { "PASSWORD", GenerateRandomPassword() } // Temporary - federated users don't need passwords
        }
      };

      // For federated authentication, we need to use a different approach
      // This is a simplified version - in production, you'd use Cognito's federated identity
      var result = await CreateOrUpdateFederatedUserAsync(tokenResponse);
      
      _logger.LogInformation("=== GOOGLE OAUTH DEBUG END - SUCCESS ===");
      return result;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "=== GOOGLE OAUTH DEBUG END - ERROR === AuthCode: {AuthCodeLength} chars, RedirectUri: {RedirectUri}, Error: {ErrorMessage}",
        authorizationCode?.Length ?? 0, redirectUri, ex.Message);
      throw;
    }
  }

  private async Task<GoogleTokenResponse> ExchangeGoogleAuthCodeAsync(string authorizationCode, string redirectUri)
  {
    var clientId = _configuration["Google:ClientId"];
    var clientSecret = _configuration["Google:ClientSecret"];
    
    _logger.LogInformation("=== TOKEN EXCHANGE DEBUG START ===");
    _logger.LogInformation("Google OAuth Configuration Check:");
    _logger.LogInformation("- ClientId: {ClientId}", string.IsNullOrEmpty(clientId) ? "MISSING" : (clientId.StartsWith("your-") ? "PLACEHOLDER" : $"SET ({clientId.Substring(0, Math.Min(20, clientId.Length))}...)"));
    _logger.LogInformation("- ClientSecret: {ClientSecret}", string.IsNullOrEmpty(clientSecret) ? "MISSING" : (clientSecret.StartsWith("your-") ? "PLACEHOLDER" : $"SET ({clientSecret.Substring(0, Math.Min(10, clientSecret.Length))}...)"));
    _logger.LogInformation("- RedirectUri: {RedirectUri}", redirectUri);
    _logger.LogInformation("- AuthCode: {AuthCodePrefix}... ({Length} chars)",
      authorizationCode?.Length > 10 ? authorizationCode.Substring(0, 10) : authorizationCode ?? "NULL",
      authorizationCode?.Length ?? 0);
    
    // Validate configuration
    if (string.IsNullOrEmpty(clientId) || clientId.StartsWith("your-"))
    {
      _logger.LogError("CONFIGURATION ERROR: Google ClientId is missing or using placeholder value");
      throw new InvalidOperationException("Google OAuth ClientId is not properly configured");
    }
    
    if (string.IsNullOrEmpty(clientSecret) || clientSecret.StartsWith("your-"))
    {
      _logger.LogError("CONFIGURATION ERROR: Google ClientSecret is missing or using placeholder value");
      throw new InvalidOperationException("Google OAuth ClientSecret is not properly configured");
    }
    
    using var httpClient = new HttpClient();
    var tokenRequest = new Dictionary<string, string>
    {
      { "code", authorizationCode },
      { "client_id", clientId },
      { "client_secret", clientSecret },
      { "redirect_uri", redirectUri },
      { "grant_type", "authorization_code" }
    };

    _logger.LogInformation("Token request parameters:");
    foreach (var param in tokenRequest)
    {
      var value = param.Key == "client_secret" ? "***HIDDEN***" :
                  param.Key == "code" ? $"{param.Value.Substring(0, Math.Min(10, param.Value.Length))}..." :
                  param.Value;
      _logger.LogInformation("  {Key}: {Value}", param.Key, value);
    }

    _logger.LogInformation("Sending token exchange request to Google OAuth endpoint: https://oauth2.googleapis.com/token");
    
    try
    {
      var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token",
        new FormUrlEncodedContent(tokenRequest));
      
      var responseContent = await response.Content.ReadAsStringAsync();
      _logger.LogInformation("Google OAuth response - Status: {StatusCode}, Content Length: {ContentLength}",
        response.StatusCode, responseContent?.Length ?? 0);
      
      if (!response.IsSuccessStatusCode)
      {
        _logger.LogError("=== GOOGLE OAUTH TOKEN EXCHANGE FAILED ===");
        _logger.LogError("Status Code: {StatusCode}", response.StatusCode);
        _logger.LogError("Response Headers: {Headers}", string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}")));
        _logger.LogError("Response Content: {Response}", responseContent);
        
        // Try to parse error details
        try
        {
          var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
          if (errorResponse.TryGetProperty("error", out var error))
          {
            _logger.LogError("Google OAuth Error: {Error}", error.GetString());
          }
          if (errorResponse.TryGetProperty("error_description", out var errorDesc))
          {
            _logger.LogError("Google OAuth Error Description: {ErrorDescription}", errorDesc.GetString());
          }
        }
        catch
        {
          _logger.LogError("Could not parse error response as JSON");
        }
        
        throw new Exception($"Failed to exchange Google authorization code for tokens. Status: {response.StatusCode}, Response: {responseContent}");
      }

      _logger.LogInformation("Token exchange successful - parsing response");

      try
      {
        var tokenResponse = JsonSerializer.Deserialize<GoogleTokenResponse>(responseContent, new JsonSerializerOptions
        {
          PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        _logger.LogInformation("Successfully parsed token response - AccessToken: {HasAccessToken}, IdToken: {HasIdToken}",
          !string.IsNullOrEmpty(tokenResponse?.AccessToken), !string.IsNullOrEmpty(tokenResponse?.IdToken));

        if (tokenResponse?.AccessToken == null)
        {
          _logger.LogError("ACCESS TOKEN IS NULL - Raw response: {Response}", responseContent);
          throw new Exception("Access token is null in Google OAuth response");
        }

        // Decode the ID token to get user info
        _logger.LogInformation("Fetching user info from Google API");
        var userInfo = await GetGoogleUserInfoAsync(tokenResponse.AccessToken);
        tokenResponse.Email = userInfo.Email;
        tokenResponse.Name = userInfo.Name;
        tokenResponse.Picture = userInfo.Picture;

        _logger.LogInformation("Successfully retrieved user info - Email: {Email}, Name: {Name}",
          userInfo.Email, userInfo.Name);
        _logger.LogInformation("=== TOKEN EXCHANGE DEBUG END - SUCCESS ===");

        return tokenResponse;
      }
      catch (JsonException ex)
      {
        _logger.LogError(ex, "Failed to parse Google OAuth response JSON: {Response}", responseContent);
        throw new Exception($"Failed to parse Google OAuth response: {ex.Message}");
      }
    }
    catch (HttpRequestException ex)
    {
      _logger.LogError(ex, "HTTP request failed when calling Google OAuth endpoint");
      throw new Exception($"Network error when calling Google OAuth: {ex.Message}");
    }
    catch (TaskCanceledException ex)
    {
      _logger.LogError(ex, "Request to Google OAuth endpoint timed out");
      throw new Exception($"Timeout when calling Google OAuth: {ex.Message}");
    }
  }

  private async Task<GoogleUserInfo> GetGoogleUserInfoAsync(string accessToken)
  {
    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Authorization =
      new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

    var response = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
    
    if (!response.IsSuccessStatusCode)
    {
      throw new Exception("Failed to get Google user info");
    }

    var jsonResponse = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<GoogleUserInfo>(jsonResponse, new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    });
  }

  private async Task<AuthResponseDto> CreateOrUpdateFederatedUserAsync(GoogleTokenResponse googleToken)
  {
    try
    {
      _logger.LogInformation("Processing federated user - Email: {Email}, Name: {Name}",
        googleToken.Email, googleToken.Name);
      
      // Try to find existing user first
      var existingUser = await FindUserByEmailAsync(googleToken.Email);
      
      if (existingUser)
      {
        _logger.LogInformation("Found existing user for email: {Email}", googleToken.Email);
        // User exists, generate JWT token for them
        return new AuthResponseDto
        {
          Token = GenerateJwtToken(googleToken.Email, googleToken.Name),
          Email = googleToken.Email,
          FirstName = GetFirstName(googleToken.Name),
          LastName = GetLastName(googleToken.Name),
          ExpiresAt = DateTime.UtcNow.AddHours(24)
        };
      }
      else
      {
        _logger.LogInformation("Creating new federated user for email: {Email}", googleToken.Email);
        // Create new user in Cognito
        await CreateFederatedUserAsync(googleToken);
        
        return new AuthResponseDto
        {
          Token = GenerateJwtToken(googleToken.Email, googleToken.Name),
          Email = googleToken.Email,
          FirstName = GetFirstName(googleToken.Name),
          LastName = GetLastName(googleToken.Name),
          ExpiresAt = DateTime.UtcNow.AddHours(24)
        };
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error creating or updating federated user for email: {Email}", googleToken?.Email ?? "NULL");
      throw;
    }
  }

  private async Task<bool> FindUserByEmailAsync(string email)
  {
    try
    {
      var request = new ListUsersRequest
      {
        UserPoolId = _configuration["AWS:Cognito:UserPoolId"],
        Filter = $"email = \"{email}\""
      };

      var response = await _cognitoClient.ListUsersAsync(request);
      return response.Users.Any();
    }
    catch
    {
      return false;
    }
  }

  private async Task CreateFederatedUserAsync(GoogleTokenResponse googleToken)
  {
    var request = new AdminCreateUserRequest
    {
      UserPoolId = _configuration["AWS:Cognito:UserPoolId"],
      Username = googleToken.Email,
      UserAttributes = new List<AttributeType>
      {
        new() { Name = "email", Value = googleToken.Email },
        new() { Name = "email_verified", Value = "true" },
        new() { Name = "name", Value = googleToken.Name }
      },
      MessageAction = MessageActionType.SUPPRESS // Don't send welcome email
    };

    await _cognitoClient.AdminCreateUserAsync(request);
  }

  private string GenerateRandomPassword()
  {
    // Generate a random password for federated users (they won't use it)
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
    var random = new Random();
    return new string(Enumerable.Repeat(chars, 12)
      .Select(s => s[random.Next(s.Length)]).ToArray());
  }

  private string GenerateJwtToken(string email, string name)
  {
    // Create a User object for JWT generation
    var nameParts = name?.Split(' ', 2) ?? new[] { "", "" };
    var user = new User
    {
      Id = Guid.NewGuid().ToString(),
      Email = email,
      UserName = email,
      FirstName = nameParts.Length > 0 ? nameParts[0] : "",
      LastName = nameParts.Length > 1 ? nameParts[1] : ""
    };
    
    return _jwtService.GenerateToken(user);
  }

  private string GetFirstName(string fullName)
  {
    if (string.IsNullOrEmpty(fullName)) return "";
    var parts = fullName.Split(' ', 2);
    return parts[0];
  }

  private string GetLastName(string fullName)
  {
    if (string.IsNullOrEmpty(fullName)) return "";
    var parts = fullName.Split(' ', 2);
    return parts.Length > 1 ? parts[1] : "";
  }

  public string GetGoogleAuthUrl(string redirectUri, string state = null)
  {
    var clientId = _configuration["Google:ClientId"];
    var scope = "openid email profile";
    
    var parameters = new Dictionary<string, string>
    {
      { "client_id", clientId },
      { "redirect_uri", redirectUri },
      { "response_type", "code" },
      { "scope", scope },
      { "access_type", "offline" }
    };

    if (!string.IsNullOrEmpty(state))
    {
      parameters.Add("state", state);
    }

    var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
    return $"https://accounts.google.com/o/oauth2/v2/auth?{queryString}";
  }
}

public class GoogleTokenResponse
{
  public string AccessToken { get; set; } = string.Empty;
  public string IdToken { get; set; } = string.Empty;
  public string RefreshToken { get; set; } = string.Empty;
  public int ExpiresIn { get; set; }
  public string Email { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string Picture { get; set; } = string.Empty;
}

public class GoogleUserInfo
{
  public string Id { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public bool VerifiedEmail { get; set; }
  public string Name { get; set; } = string.Empty;
  public string GivenName { get; set; } = string.Empty;
  public string FamilyName { get; set; } = string.Empty;
  public string Picture { get; set; } = string.Empty;
}