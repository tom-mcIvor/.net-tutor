using System.ComponentModel.DataAnnotations;

namespace DotNetTutor.Api.Models;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool RequiresVerification { get; set; }
}

public class ConfirmSignUpDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string ConfirmationCode { get; set; } = string.Empty;
}

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string ConfirmationCode { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
}

public class ResendConfirmationDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class GoogleOAuthDto
{
    [Required]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    public string RedirectUri { get; set; } = string.Empty;
    
    public string? State { get; set; }
}