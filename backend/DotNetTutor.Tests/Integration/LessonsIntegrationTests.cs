using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System.Text.Json;
using DotNetTutor.Api.Data;
using DotNetTutor.Api.Controllers;

namespace DotNetTutor.Tests.Integration;

public class LessonsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public LessonsIntegrationTests(WebApplicationFactory<Program> factory)
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
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllLessons_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/lessons");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
    }

    [Fact]
    public async Task GetAllLessons_ReturnsLessonsArray()
    {
        // Act
        var response = await _client.GetAsync("/api/lessons");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var lessons = JsonSerializer.Deserialize<LessonDto[]>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        lessons.Should().NotBeNull();
        lessons.Should().NotBeEmpty();
        lessons!.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Title));
        lessons.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Description));
        lessons.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Content));
    }

    [Fact]
    public async Task GetLessonById_WithValidId_ReturnsLesson()
    {
        // Arrange
        var lessonId = 1;

        // Act
        var response = await _client.GetAsync($"/api/lessons/{lessonId}");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var lesson = JsonSerializer.Deserialize<LessonDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        lesson.Should().NotBeNull();
        lesson!.Id.Should().Be(lessonId);
        lesson.Title.Should().NotBeNullOrEmpty();
        lesson.Description.Should().NotBeNullOrEmpty();
        lesson.Content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetLessonById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999999;

        // Act
        var response = await _client.GetAsync($"/api/lessons/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetLessonCount_ReturnsCount()
    {
        // Act
        var response = await _client.GetAsync("/api/lessons/count");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var countObject = JsonSerializer.Deserialize<JsonElement>(content);
        
        countObject.TryGetProperty("count", out var countProperty).Should().BeTrue();
        var count = countProperty.GetInt32();
        count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetTopics_ReturnsTopics()
    {
        // Act
        var response = await _client.GetAsync("/api/lessons/topics");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var topics = JsonSerializer.Deserialize<JsonElement[]>(content);
        
        topics.Should().NotBeNull();
        topics.Should().NotBeEmpty();
        
        foreach (var topic in topics!)
        {
            topic.TryGetProperty("id", out _).Should().BeTrue();
            topic.TryGetProperty("title", out _).Should().BeTrue();
            topic.TryGetProperty("description", out _).Should().BeTrue();
        }
    }

    [Fact]
    public async Task GetAspNetCoreLessons_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/lessons/aspnetcore");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
    }

    [Fact]
    public async Task GetAspNetCoreLessons_ReturnsLessonsArray()
    {
        // Act
        var response = await _client.GetAsync("/api/lessons/aspnetcore");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var lessons = JsonSerializer.Deserialize<LessonDto[]>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        lessons.Should().NotBeNull();
        lessons.Should().NotBeEmpty();
        lessons!.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Title));
        lessons.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Description));
        lessons.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Content));
    }

    [Fact]
    public async Task GetAspNetCoreLessonById_WithValidId_ReturnsLesson()
    {
        // Arrange
        var lessonId = 10; // Known ASP.NET Core lesson ID

        // Act
        var response = await _client.GetAsync($"/api/lessons/aspnetcore/{lessonId}");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var lesson = JsonSerializer.Deserialize<LessonDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        lesson.Should().NotBeNull();
        lesson!.Id.Should().Be(lessonId);
        lesson.Title.Should().NotBeNullOrEmpty();
        lesson.Description.Should().NotBeNullOrEmpty();
        lesson.Content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAspNetCoreLessonById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999999;

        // Act
        var response = await _client.GetAsync($"/api/lessons/aspnetcore/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAspNetCoreLessonCount_ReturnsCount()
    {
        // Act
        var response = await _client.GetAsync("/api/lessons/aspnetcore/count");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var countObject = JsonSerializer.Deserialize<JsonElement>(content);
        
        countObject.TryGetProperty("count", out var countProperty).Should().BeTrue();
        var count = countProperty.GetInt32();
        count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetAspNetCoreTopics_ReturnsTopics()
    {
        // Act
        var response = await _client.GetAsync("/api/lessons/aspnetcore/topics");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var topics = JsonSerializer.Deserialize<JsonElement[]>(content);
        
        topics.Should().NotBeNull();
        topics.Should().NotBeEmpty();
        
        foreach (var topic in topics!)
        {
            topic.TryGetProperty("id", out _).Should().BeTrue();
            topic.TryGetProperty("title", out _).Should().BeTrue();
            topic.TryGetProperty("description", out _).Should().BeTrue();
        }
    }

    [Fact]
    public async Task GetFromDatabase_ReturnsSuccessAndEmptyArray()
    {
        // Act
        var response = await _client.GetAsync("/api/lessons/database");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var lessons = JsonSerializer.Deserialize<JsonElement[]>(content);
        
        lessons.Should().NotBeNull();
        // Should be empty since we're using in-memory database without seeding
        lessons.Should().BeEmpty();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetLessonById_WithMultipleValidIds_ReturnsCorrectLessons(int lessonId)
    {
        // Act
        var response = await _client.GetAsync($"/api/lessons/{lessonId}");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var lesson = JsonSerializer.Deserialize<LessonDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        lesson.Should().NotBeNull();
        lesson!.Id.Should().Be(lessonId);
    }

    [Theory]
    [InlineData("/api/lessons")]
    [InlineData("/api/lessons/count")]
    [InlineData("/api/lessons/topics")]
    [InlineData("/api/lessons/aspnetcore")]
    [InlineData("/api/lessons/aspnetcore/count")]
    [InlineData("/api/lessons/aspnetcore/topics")]
    [InlineData("/api/lessons/database")]
    public async Task AllEndpoints_ReturnSuccessAndCorrectContentType(string endpoint)
    {
        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(100000)]
    public async Task GetLessonById_WithInvalidIds_ReturnsNotFound(int invalidId)
    {
        // Act
        var response = await _client.GetAsync($"/api/lessons/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}