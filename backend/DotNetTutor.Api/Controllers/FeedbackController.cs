using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetTutor.Api.Data;
using DotNetTutor.Api.Models;
using System.Security.Claims;

namespace DotNetTutor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FeedbackController> _logger;

    public FeedbackController(ApplicationDbContext context, ILogger<FeedbackController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> CreateFeedback(CreateFeedbackDto feedbackDto)
    {
        try
        {
            var feedback = new Feedback
            {
                Message = feedbackDto.Message,
                PageContext = feedbackDto.PageContext,
                CreatedAt = DateTime.UtcNow
            };

            // Get user information if authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                feedback.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                feedback.UserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            }

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Feedback created with ID: {FeedbackId}", feedback.Id);

            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.Id }, feedback);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating feedback");
            return StatusCode(500, "An error occurred while saving feedback");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Feedback>> GetFeedback(int id)
    {
        var feedback = await _context.Feedbacks.FindAsync(id);

        if (feedback == null)
        {
            return NotFound();
        }

        return feedback;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedback()
    {
        return await _context.Feedbacks
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }
}

public class CreateFeedbackDto
{
    public string Message { get; set; } = string.Empty;
    public string? PageContext { get; set; }
}