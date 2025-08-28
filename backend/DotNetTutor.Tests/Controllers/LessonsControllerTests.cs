using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using DotNetTutor.Api.Controllers;
using DotNetTutor.Api.Data;

namespace DotNetTutor.Tests.Controllers;

public class LessonsControllerTests
{
    private ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public void GetAll_ReturnsOkResultWithCSharpLessons()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);

        // Act
        var result = controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var lessons = okResult!.Value as LessonDto[];
        
        lessons.Should().NotBeNull();
        lessons.Should().NotBeEmpty();
        lessons!.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Title));
        lessons.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Description));
        lessons.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Content));
    }

    [Fact]
    public void GetById_WithValidId_ReturnsOkResultWithLesson()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);
        var validId = 1; // Assuming ID 1 exists in C# basics lessons

        // Act
        var result = controller.GetById(validId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var lesson = okResult!.Value as LessonDto;
        
        lesson.Should().NotBeNull();
        lesson!.Id.Should().Be(validId);
        lesson.Title.Should().NotBeNullOrEmpty();
        lesson.Description.Should().NotBeNullOrEmpty();
        lesson.Content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);
        var invalidId = 999999;

        // Act
        var result = controller.GetById(invalidId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void GetLessonCount_ReturnsOkResultWithCount()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);

        // Act
        var result = controller.GetLessonCount();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        
        var countObject = okResult!.Value;
        countObject.Should().NotBeNull();
        
        // Use reflection to check the count property
        var countProperty = countObject!.GetType().GetProperty("count");
        countProperty.Should().NotBeNull();
        var count = (int)countProperty!.GetValue(countObject)!;
        count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetTopics_ReturnsOkResultWithTopics()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);

        // Act
        var result = controller.GetTopics();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var topics = okResult!.Value;
        
        topics.Should().NotBeNull();
        
        // Convert to array of objects and verify structure
        var topicsArray = topics as object[];
        topicsArray.Should().NotBeNull();
        topicsArray.Should().NotBeEmpty();
        
        // Check that each topic has Id, Title, and Description
        foreach (var topic in topicsArray!)
        {
            var topicType = topic.GetType();
            topicType.GetProperty("Id").Should().NotBeNull();
            topicType.GetProperty("Title").Should().NotBeNull();
            topicType.GetProperty("Description").Should().NotBeNull();
        }
    }

    [Fact]
    public void GetAspNetCoreLessons_ReturnsOkResultWithLessons()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);

        // Act
        var result = controller.GetAspNetCoreLessons();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var lessons = okResult!.Value as LessonDto[];
        
        lessons.Should().NotBeNull();
        lessons.Should().NotBeEmpty();
        lessons!.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Title));
        lessons.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Description));
        lessons.Should().OnlyContain(l => !string.IsNullOrEmpty(l.Content));
    }

    [Fact]
    public void GetAspNetCoreLessonById_WithValidId_ReturnsOkResultWithLesson()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);
        var validId = 10; // Assuming ID 10 exists in ASP.NET Core lessons

        // Act
        var result = controller.GetAspNetCoreLessonById(validId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var lesson = okResult!.Value as LessonDto;
        
        lesson.Should().NotBeNull();
        lesson!.Id.Should().Be(validId);
        lesson.Title.Should().NotBeNullOrEmpty();
        lesson.Description.Should().NotBeNullOrEmpty();
        lesson.Content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetAspNetCoreLessonById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);
        var invalidId = 999999;

        // Act
        var result = controller.GetAspNetCoreLessonById(invalidId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void GetAspNetCoreLessonCount_ReturnsOkResultWithCount()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);

        // Act
        var result = controller.GetAspNetCoreLessonCount();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        
        var countObject = okResult!.Value;
        countObject.Should().NotBeNull();
        
        // Use reflection to check the count property
        var countProperty = countObject!.GetType().GetProperty("count");
        countProperty.Should().NotBeNull();
        var count = (int)countProperty!.GetValue(countObject)!;
        count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetAspNetCoreTopics_ReturnsOkResultWithTopics()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);

        // Act
        var result = controller.GetAspNetCoreTopics();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var topics = okResult!.Value;
        
        topics.Should().NotBeNull();
        
        // Convert to array of objects and verify structure
        var topicsArray = topics as object[];
        topicsArray.Should().NotBeNull();
        topicsArray.Should().NotBeEmpty();
        
        // Check that each topic has Id, Title, and Description
        foreach (var topic in topicsArray!)
        {
            var topicType = topic.GetType();
            topicType.GetProperty("Id").Should().NotBeNull();
            topicType.GetProperty("Title").Should().NotBeNull();
            topicType.GetProperty("Description").Should().NotBeNull();
        }
    }

    [Fact]
    public async Task GetFromDatabase_ReturnsOkResultWithEmptyList()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);

        // Act
        var result = await controller.GetFromDatabase();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var lessons = okResult!.Value;
        
        lessons.Should().NotBeNull();
        // Since we're using in-memory database without seeding data,
        // this should return an empty list
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetById_WithKnownIds_ReturnsCorrectLessons(int lessonId)
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);

        // Act
        var result = controller.GetById(lessonId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var lesson = okResult!.Value as LessonDto;
        
        lesson.Should().NotBeNull();
        lesson!.Id.Should().Be(lessonId);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(100000)]
    public void GetById_WithInvalidIds_ReturnsNotFound(int invalidId)
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var controller = new LessonsController(context);

        // Act
        var result = controller.GetById(invalidId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}