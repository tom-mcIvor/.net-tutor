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
      var signUpRequest = new SignUpRequest
      {
        ClientId = _configuration["AWS:Cognito:ClientId"],
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
      }

      var response = await _cognitoClient.SignUpAsync(signUpRequest);

      return new AuthResponseDto
      {
        Email = registerDto.Email,
        FirstName = registerDto.FirstName,
        LastName = registerDto.LastName,
        RequiresVerification = !response.UserConfirmed
      };
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error registering user {Email}", registerDto.Email);
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
      // Exchange authorization code for tokens with Google
      var tokenResponse = await ExchangeGoogleAuthCodeAsync(authorizationCode, redirectUri);
      
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
      return await CreateOrUpdateFederatedUserAsync(tokenResponse);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error handling Google OAuth");
      throw;
    }
  }

  private async Task<GoogleTokenResponse> ExchangeGoogleAuthCodeAsync(string authorizationCode, string redirectUri)
  {
    var clientId = _configuration["Google:ClientId"];
    var clientSecret = _configuration["Google:ClientSecret"];
    
    using var httpClient = new HttpClient();
    var tokenRequest = new Dictionary<string, string>
    {
      { "code", authorizationCode },
      { "client_id", clientId },
      { "client_secret", clientSecret },
      { "redirect_uri", redirectUri },
      { "grant_type", "authorization_code" }
    };

    var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token",
      new FormUrlEncodedContent(tokenRequest));
    
    if (!response.IsSuccessStatusCode)
    {
      throw new Exception("Failed to exchange Google authorization code for tokens");
    }

    var jsonResponse = await response.Content.ReadAsStringAsync();
    var tokenResponse = JsonSerializer.Deserialize<GoogleTokenResponse>(jsonResponse, new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    });

    // Decode the ID token to get user info
    var userInfo = await GetGoogleUserInfoAsync(tokenResponse.AccessToken);
    tokenResponse.Email = userInfo.Email;
    tokenResponse.Name = userInfo.Name;
    tokenResponse.Picture = userInfo.Picture;

    return tokenResponse;
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
      // Try to find existing user first
      var existingUser = await FindUserByEmailAsync(googleToken.Email);
      
      if (existingUser != null)
      {
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
      _logger.LogError(ex, "Error creating or updating federated user");
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