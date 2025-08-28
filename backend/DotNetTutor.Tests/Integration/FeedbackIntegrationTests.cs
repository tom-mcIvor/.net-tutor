using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System.Text;
using System.Text.Json;
using DotNetTutor.Api.Data;
using DotNetTutor.Api.Models;
using DotNetTutor.Api.Controllers;

namespace DotNetTutor.Tests.Integration;

public class FeedbackIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public FeedbackIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("FeedbackTestDb");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task PostFeedback_WithValidData_ReturnsCreated()
    {
        // Arrange
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "This is test feedback",
            PageContext = "/lessons/1"
        };

        var json = JsonSerializer.Serialize(feedbackDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/feedback", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var responseContent = await response.Content.ReadAsStringAsync();
        var feedback = JsonSerializer.Deserialize<Feedback>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        feedback.Should().NotBeNull();
        feedback!.Id.Should().BeGreaterThan(0);
        feedback.Message.Should().Be(feedbackDto.Message);
        feedback.PageContext.Should().Be(feedbackDto.PageContext);
        feedback.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task PostFeedback_WithEmptyMessage_StillCreates()
    {
        // Arrange
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "",
            PageContext = "/test"
        };

        var json = JsonSerializer.Serialize(feedbackDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/feedback", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        var feedback = JsonSerializer.Deserialize<Feedback>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        feedback.Should().NotBeNull();
        feedback!.Message.Should().Be("");
    }

    [Fact]
    public async Task PostFeedback_WithNullPageContext_StillCreates()
    {
        // Arrange
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "Test message",
            PageContext = null
        };

        var json = JsonSerializer.Serialize(feedbackDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/feedback", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        var feedback = JsonSerializer.Deserialize<Feedback>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        feedback.Should().NotBeNull();
        feedback!.PageContext.Should().BeNull();
    }

    [Fact]
    public async Task PostFeedback_WithInvalidJson_ReturnsBadRequest()
    {
        // Arrange
        var invalidJson = "{ invalid json }";
        var content = new StringContent(invalidJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/feedback", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetFeedback_WithValidId_ReturnsOk()
    {
        // Arrange - Create feedback first
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "Test feedback for retrieval",
            PageContext = "/test"
        };

        var json = JsonSerializer.Serialize(feedbackDto);
        var createContent = new StringContent(json, Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync("/api/feedback", createContent);
        
        var createResponseContent = await createResponse.Content.ReadAsStringAsync();
        var createdFeedback = JsonSerializer.Deserialize<Feedback>(createResponseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Act
        var response = await _client.GetAsync($"/api/feedback/{createdFeedback!.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var feedback = JsonSerializer.Deserialize<Feedback>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        feedback.Should().NotBeNull();
        feedback!.Id.Should().Be(createdFeedback.Id);
        feedback.Message.Should().Be(feedbackDto.Message);
        feedback.PageContext.Should().Be(feedbackDto.PageContext);
    }

    [Fact]
    public async Task GetFeedback_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999999;

        // Act
        var response = await _client.GetAsync($"/api/feedback/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllFeedback_ReturnsAllFeedbackOrderedByDate()
    {
        // Arrange - Create multiple feedback entries
        var feedbackItems = new[]
        {
            new CreateFeedbackDto { Message = "First feedback", PageContext = "/test1" },
            new CreateFeedbackDto { Message = "Second feedback", PageContext = "/test2" },
            new CreateFeedbackDto { Message = "Third feedback", PageContext = "/test3" }
        };

        // Create feedback entries with small delays to ensure different timestamps
        foreach (var item in feedbackItems)
        {
            var json = JsonSerializer.Serialize(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/feedback", content);
            await Task.Delay(10); // Small delay to ensure different timestamps
        }

        // Act
        var response = await _client.GetAsync("/api/feedback");

        // Assert
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var feedbacks = JsonSerializer.Deserialize<Feedback[]>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        feedbacks.Should().NotBeNull();
        feedbacks!.Length.Should().BeGreaterOrEqualTo(3);
        
        // Should be ordered by CreatedAt descending
        for (int i = 0; i < feedbacks.Length - 1; i++)
        {
            feedbacks[i].CreatedAt.Should().BeOnOrAfter(feedbacks[i + 1].CreatedAt);
        }
    }

    [Fact]
    public async Task GetAllFeedback_WithNoFeedback_ReturnsEmptyArray()
    {
        // Arrange - Use a fresh database by changing the database name
        using var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("EmptyFeedbackTestDb");
                });
            });
        });

        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/feedback");

        // Assert
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var feedbacks = JsonSerializer.Deserialize<Feedback[]>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        feedbacks.Should().NotBeNull();
        feedbacks!.Should().BeEmpty();
    }

    [Fact]
    public async Task PostFeedback_MultipleRequests_AllSucceed()
    {
        // Arrange
        var feedbackDtos = Enumerable.Range(1, 5)
            .Select(i => new CreateFeedbackDto
            {
                Message = $"Test feedback {i}",
                PageContext = $"/test/{i}"
            }).ToArray();

        // Act & Assert
        foreach (var dto in feedbackDtos)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/feedback", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        // Verify all were created
        var getAllResponse = await _client.GetAsync("/api/feedback");
        var responseContent = await getAllResponse.Content.ReadAsStringAsync();
        var allFeedbacks = JsonSerializer.Deserialize<Feedback[]>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        allFeedbacks.Should().NotBeNull();
        allFeedbacks!.Length.Should().BeGreaterOrEqualTo(5);
    }

    [Fact]
    public async Task FeedbackEndpoints_ReturnCorrectContentType()
    {
        // Arrange
        var feedbackDto = new CreateFeedbackDto
        {
            Message = "Test for content type",
            PageContext = "/test"
        };

        var json = JsonSerializer.Serialize(feedbackDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Create feedback
        var createResponse = await _client.PostAsync("/api/feedback", content);
        var createResponseContent = await createResponse.Content.ReadAsStringAsync();
        var createdFeedback = JsonSerializer.Deserialize<Feedback>(createResponseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Test individual feedback endpoint
        var getResponse = await _client.GetAsync($"/api/feedback/{createdFeedback!.Id}");
        getResponse.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");

        // Test all feedback endpoint
        var getAllResponse = await _client.GetAsync("/api/feedback");
        getAllResponse.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
    }

    [Theory]
    [InlineData("Very long feedback message that contains lots of text to test how the system handles longer messages that users might submit through the feedback form")]
    [InlineData("Short")]
    [InlineData("Message with special characters: !@#$%^&*()")]
    [InlineData("Message with unicode: 🚀 🌟 ✨")]
    public async Task PostFeedback_WithVariousMessageTypes_AllSucceed(string message)
    {
        // Arrange
        var feedbackDto = new CreateFeedbackDto
        {
            Message = message,
            PageContext = "/test"
        };

        var json = JsonSerializer.Serialize(feedbackDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/feedback", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        var feedback = JsonSerializer.Deserialize<Feedback>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        feedback.Should().NotBeNull();
        feedback!.Message.Should().Be(message);
    }
}