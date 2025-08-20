using System.ComponentModel.DataAnnotations;

namespace DotNetTutor.Api.Models;

public class Feedback
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;
    
    public string? UserId { get; set; }
    
    public string? UserEmail { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public string? PageContext { get; set; } // Which page/tab the feedback was submitted from
    
    // Navigation property to User if needed
    public User? User { get; set; }
}