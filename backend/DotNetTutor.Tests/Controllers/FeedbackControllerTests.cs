using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using DotNetTutor.Api.Controllers;
using DotNetTutor.Api.Data;
using DotNetTutor.Api.Models;

namespace DotNetTutor.Tests.Controllers;

public class FeedbackControllerTests
{
    private readonly Mock<ILogger<FeedbackController>> _mockLogger;

    public FeedbackControllerTests()
    {
        _mockLogger = new Mock<ILogger<FeedbackController>>();
    }

    private ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    private FeedbackController CreateControllerWithUser(ApplicationDbContext context, bool isAuthenticated = false, string? userId = null, string? userEmail = null)
    {
        var controller = new FeedbackController(context, _mockLogger.Object);

        // Set up user context
        var claims = new List<Claim>();
        if (isAuthenticated)
        {
            if (userId != null)
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            if (userEmail != null)
                claims.Add(new Claim(ClaimTypes.Email, userEmail));
        }

        var identity = new ClaimsIdentity(claims, isAuthenticated ? "Test" : null);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
        };

        return controller;
    }

    [Fact]
    public async Task CreateFeedback_WithValidData_ReturnsCreatedResult()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context);
        
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "Great tutorial!",
            PageContext = "/lessons/1"
        };

        // Act
        var result = await controller.CreateFeedback(feedbackDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        
        createdResult!.ActionName.Should().Be(nameof(controller.GetFeedback));
        
        var feedback = createdResult.Value as Feedback;
        feedback.Should().NotBeNull();
        feedback!.Message.Should().Be(feedbackDto.Message);
        feedback.PageContext.Should().Be(feedbackDto.PageContext);
        feedback.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        feedback.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateFeedback_WithAuthenticatedUser_SetUserInformation()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var userId = "test-user-123";
        var userEmail = "test@example.com";
        var controller = CreateControllerWithUser(context, true, userId, userEmail);
        
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "Great tutorial!",
            PageContext = "/lessons/1"
        };

        // Act
        var result = await controller.CreateFeedback(feedbackDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        
        var feedback = createdResult!.Value as Feedback;
        feedback.Should().NotBeNull();
        feedback!.UserId.Should().Be(userId);
        feedback.UserEmail.Should().Be(userEmail);
    }

    [Fact]
    public async Task CreateFeedback_WithUnauthenticatedUser_DoesNotSetUserInformation()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context, false);
        
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "Great tutorial!",
            PageContext = "/lessons/1"
        };

        // Act
        var result = await controller.CreateFeedback(feedbackDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        
        var feedback = createdResult!.Value as Feedback;
        feedback.Should().NotBeNull();
        feedback!.UserId.Should().BeNull();
        feedback.UserEmail.Should().BeNull();
    }

    [Fact]
    public async Task CreateFeedback_SavesToDatabase()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context);
        
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "Test feedback",
            PageContext = "/test"
        };

        // Act
        await controller.CreateFeedback(feedbackDto);

        // Assert
        var savedFeedback = await context.Feedbacks.FirstOrDefaultAsync();
        savedFeedback.Should().NotBeNull();
        savedFeedback!.Message.Should().Be(feedbackDto.Message);
        savedFeedback.PageContext.Should().Be(feedbackDto.PageContext);
    }

    [Fact]
    public async Task GetFeedback_WithValidId_ReturnsOkResult()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context);
        
        var feedback = new Feedback
        {
            Message = "Test feedback",
            PageContext = "/test",
            CreatedAt = DateTime.UtcNow
        };
        
        context.Feedbacks.Add(feedback);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.GetFeedback(feedback.Id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var returnedFeedback = okResult!.Value as Feedback;
        
        returnedFeedback.Should().NotBeNull();
        returnedFeedback!.Id.Should().Be(feedback.Id);
        returnedFeedback.Message.Should().Be(feedback.Message);
        returnedFeedback.PageContext.Should().Be(feedback.PageContext);
    }

    [Fact]
    public async Task GetFeedback_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context);
        var invalidId = 999999;

        // Act
        var result = await controller.GetFeedback(invalidId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAllFeedback_ReturnsAllFeedbackOrderedByDate()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context);
        
        var feedback1 = new Feedback
        {
            Message = "First feedback",
            CreatedAt = DateTime.UtcNow.AddHours(-2)
        };
        
        var feedback2 = new Feedback
        {
            Message = "Second feedback",
            CreatedAt = DateTime.UtcNow.AddHours(-1)
        };
        
        var feedback3 = new Feedback
        {
            Message = "Third feedback",
            CreatedAt = DateTime.UtcNow
        };
        
        context.Feedbacks.AddRange(feedback1, feedback2, feedback3);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.GetAllFeedback();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var feedbacks = okResult!.Value as List<Feedback>;
        
        feedbacks.Should().NotBeNull();
        feedbacks!.Should().HaveCount(3);
        
        // Should be ordered by CreatedAt descending (most recent first)
        feedbacks[0].Message.Should().Be("Third feedback");
        feedbacks[1].Message.Should().Be("Second feedback");
        feedbacks[2].Message.Should().Be("First feedback");
    }

    [Fact]
    public async Task GetAllFeedback_WithNoFeedback_ReturnsEmptyList()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context);

        // Act
        var result = await controller.GetAllFeedback();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var feedbacks = okResult!.Value as List<Feedback>;
        
        feedbacks.Should().NotBeNull();
        feedbacks!.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateFeedback_WithEmptyMessage_StillCreatesRecord(string message)
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context);
        
        var feedbackDto = new CreateFeedbackDto
        {
            Message = message,
            PageContext = "/test"
        };

        // Act
        var result = await controller.CreateFeedback(feedbackDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        
        var feedback = createdResult!.Value as Feedback;
        feedback.Should().NotBeNull();
        feedback!.Message.Should().Be(message);
    }

    [Fact]
    public async Task CreateFeedback_WithNullPageContext_StillCreatesRecord()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context);
        
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "Test message",
            PageContext = null
        };

        // Act
        var result = await controller.CreateFeedback(feedbackDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        
        var feedback = createdResult!.Value as Feedback;
        feedback.Should().NotBeNull();
        feedback!.PageContext.Should().BeNull();
    }

    [Fact]
    public async Task CreateFeedback_LogsInformation()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = CreateControllerWithUser(context);
        
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "Test feedback",
            PageContext = "/test"
        };

        // Act
        await controller.CreateFeedback(feedbackDto);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Feedback created with ID")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}