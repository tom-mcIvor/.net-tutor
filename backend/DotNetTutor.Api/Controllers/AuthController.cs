using Microsoft.AspNetCore.Mvc;
using DotNetTutor.Api.Models;
using DotNetTutor.Api.Services;
using Amazon.CognitoIdentityProvider.Model;

namespace DotNetTutor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly CognitoService _cognitoService;
  private readonly ILogger<AuthController> _logger;

  public AuthController(CognitoService cognitoService, ILogger<AuthController> logger)
  {
    _cognitoService = cognitoService;
    _logger = logger;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var result = await _cognitoService.RegisterAsync(registerDto);
      return Ok(result);
    }
    catch (UsernameExistsException)
    {
      return BadRequest(new { message = "A user with this email already exists." });
    }
    catch (InvalidPasswordException ex)
    {
      return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Registration failed for {Email}", registerDto.Email);
      return BadRequest(new { message = "Registration failed. Please try again." });
    }
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var result = await _cognitoService.LoginAsync(loginDto);
      return Ok(result);
    }
    catch (UserNotConfirmedException)
    {
      return BadRequest(new
      {
        message = "Please verify your email address before signing in.",
        requiresVerification = true,
        email = loginDto.Email
      });
    }
    catch (NotAuthorizedException)
    {
      return Unauthorized(new { message = "Invalid email or password." });
    }
    catch (UserNotFoundException)
    {
      return Unauthorized(new { message = "Invalid email or password." });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Login failed for {Email}", loginDto.Email);
      return BadRequest(new { message = "Login failed. Please try again." });
    }
  }

  [HttpPost("confirm-signup")]
  public async Task<IActionResult> ConfirmSignUp([FromBody] ConfirmSignUpDto confirmDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      await _cognitoService.ConfirmSignUpAsync(confirmDto);
      return Ok(new { message = "Email verified successfully. You can now sign in." });
    }
    catch (CodeMismatchException)
    {
      return BadRequest(new { message = "Invalid verification code." });
    }
    catch (ExpiredCodeException)
    {
      return BadRequest(new { message = "Verification code has expired. Please request a new one." });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Email confirmation failed for {Email}", confirmDto.Email);
      return BadRequest(new { message = "Email verification failed. Please try again." });
    }
  }

  [HttpPost("resend-confirmation")]
  public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationDto resendDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      await _cognitoService.ResendConfirmationCodeAsync(resendDto);
      return Ok(new { message = "Verification code sent to your email." });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Resend confirmation failed for {Email}", resendDto.Email);
      return BadRequest(new { message = "Failed to send verification code. Please try again." });
    }
  }

  [HttpPost("forgot-password")]
  public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      await _cognitoService.ForgotPasswordAsync(forgotPasswordDto);
      return Ok(new { message = "Password reset code sent to your email." });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Forgot password failed for {Email}", forgotPasswordDto.Email);
      return BadRequest(new { message = "Failed to send password reset code. Please try again." });
    }
  }

  [HttpPost("reset-password")]
  public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      await _cognitoService.ConfirmForgotPasswordAsync(resetPasswordDto);
      return Ok(new { message = "Password reset successfully. You can now sign in with your new password." });
    }
    catch (CodeMismatchException)
    {
      return BadRequest(new { message = "Invalid reset code." });
    }
    catch (ExpiredCodeException)
    {
      return BadRequest(new { message = "Reset code has expired. Please request a new one." });
    }
    catch (InvalidPasswordException ex)
    {
      return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Password reset failed for {Email}", resetPasswordDto.Email);
      return BadRequest(new { message = "Password reset failed. Please try again." });
    }
  }

  [HttpPost("refresh-token")]
  public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var result = await _cognitoService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
      return Ok(result);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Token refresh failed");
      return Unauthorized(new { message = "Token refresh failed. Please sign in again." });
    }
  }

  [HttpGet("google-auth-url")]
  public IActionResult GetGoogleAuthUrl([FromQuery] string redirectUri, [FromQuery] string? state = null)
  {
    try
    {
      var authUrl = _cognitoService.GetGoogleAuthUrl(redirectUri, state);
      return Ok(new { authUrl });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error generating Google auth URL");
      return BadRequest(new { message = "Failed to generate Google auth URL" });
    }
  }

  [HttpPost("google-oauth")]
  public async Task<IActionResult> GoogleOAuth([FromBody] GoogleOAuthDto googleOAuthDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var result = await _cognitoService.HandleGoogleOAuthAsync(googleOAuthDto.Code, googleOAuthDto.RedirectUri);
      return Ok(result);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Google OAuth failed");
      return BadRequest(new { message = "Google OAuth authentication failed. Please try again." });
    }
  }

  [HttpPost("logout")]
  public async Task<IActionResult> Logout()
  {
    // With Cognito, logout is handled client-side by clearing tokens
    // No server-side action needed unless using global sign-out
    return Ok(new { message = "Logged out successfully" });
  }
}

public class RefreshTokenDto
{
  public string RefreshToken { get; set; } = string.Empty;
}