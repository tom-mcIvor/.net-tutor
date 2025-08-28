using FluentAssertions;
using DotNetTutor.Api.Services;
using DotNetTutor.Api.Controllers;

namespace DotNetTutor.Tests.Services;

public class LessonContentServiceTests
{
    [Fact]
    public void CSharpBasicsLessons_ShouldNotBeEmpty()
    {
        // Act
        var lessons = LessonContentService.CSharpBasicsLessons;

        // Assert
        lessons.Should().NotBeEmpty();
        lessons.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public void AspNetCoreLessons_ShouldNotBeEmpty()
    {
        // Act
        var lessons = LessonContentService.AspNetCoreLessons;

        // Assert
        lessons.Should().NotBeEmpty();
        lessons.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public void CSharpBasicsLessons_ShouldHaveValidStructure()
    {
        // Act
        var lessons = LessonContentService.CSharpBasicsLessons;

        // Assert
        foreach (var lesson in lessons)
        {
            lesson.Id.Should().BeGreaterThan(0);
            lesson.Title.Should().NotBeNullOrEmpty();
            lesson.Description.Should().NotBeNullOrEmpty();
            lesson.Content.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public void AspNetCoreLessons_ShouldHaveValidStructure()
    {
        // Act
        var lessons = LessonContentService.AspNetCoreLessons;

        // Assert
        foreach (var lesson in lessons)
        {
            lesson.Id.Should().BeGreaterThan(0);
            lesson.Title.Should().NotBeNullOrEmpty();
            lesson.Description.Should().NotBeNullOrEmpty();
            lesson.Content.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public void CSharpBasicsLessons_ShouldHaveUniqueIds()
    {
        // Act
        var lessons = LessonContentService.CSharpBasicsLessons;
        var ids = lessons.Select(l => l.Id).ToList();

        // Assert
        ids.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void AspNetCoreLessons_ShouldHaveUniqueIds()
    {
        // Act
        var lessons = LessonContentService.AspNetCoreLessons;
        var ids = lessons.Select(l => l.Id).ToList();

        // Assert
        ids.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void CSharpBasicsLessons_ShouldStartWithWelcomeLesson()
    {
        // Act
        var lessons = LessonContentService.CSharpBasicsLessons;
        var firstLesson = lessons.First();

        // Assert
        firstLesson.Id.Should().Be(1);
        firstLesson.Title.Should().ContainEquivalentOf("Welcome");
    }

    [Fact]
    public void AspNetCoreLessons_ShouldContainMvcLesson()
    {
        // Act
        var lessons = LessonContentService.AspNetCoreLessons;

        // Assert
        lessons.Should().Contain(l => l.Title.ToLowerInvariant().Contains("mvc"));
    }

    [Fact]
    public void CSharpBasicsLessons_ShouldContainVariablesLesson()
    {
        // Act
        var lessons = LessonContentService.CSharpBasicsLessons;

        // Assert
        lessons.Should().Contain(l => l.Title.ToLowerInvariant().Contains("variables"));
    }

    [Fact]
    public void AllLessons_ShouldHaveMarkdownContent()
    {
        // Arrange
        var allLessons = LessonContentService.CSharpBasicsLessons
            .Concat(LessonContentService.AspNetCoreLessons);

        // Act & Assert
        foreach (var lesson in allLessons)
        {
            // Content should contain markdown elements like headers (#), code blocks (```)
            lesson.Content.Should().ContainAny("#", "```", "*", "-");
        }
    }

    [Theory]
    [InlineData(1, "Welcome to C# Programming")]
    [InlineData(2, "Variables and Data Types")]
    [InlineData(3, "Operators and Expressions")]
    public void CSharpBasicsLessons_ShouldContainExpectedLessons(int expectedId, string expectedTitlePart)
    {
        // Act
        var lessons = LessonContentService.CSharpBasicsLessons;
        var lesson = lessons.FirstOrDefault(l => l.Id == expectedId);

        // Assert
        lesson.Should().NotBeNull();
        lesson!.Title.Should().ContainEquivalentOf(expectedTitlePart);
    }

    [Theory]
    [InlineData(10, "MVC")]
    [InlineData(11, "Dependency Injection")]
    public void AspNetCoreLessons_ShouldContainExpectedLessons(int expectedId, string expectedTitlePart)
    {
        // Act
        var lessons = LessonContentService.AspNetCoreLessons;
        var lesson = lessons.FirstOrDefault(l => l.Id == expectedId);

        // Assert
        lesson.Should().NotBeNull();
        lesson!.Title.Should().ContainEquivalentOf(expectedTitlePart);
    }

    [Fact]
    public void CSharpBasicsLessons_ShouldHaveProgressiveComplexity()
    {
        // Act
        var lessons = LessonContentService.CSharpBasicsLessons;

        // Assert
        // First lesson should be introductory
        var firstLesson = lessons.First();
        firstLesson.Description.Should().ContainAny("Introduction", "Welcome", "Getting Started");

        // Should progress from basic concepts to more advanced
        var variablesLesson = lessons.FirstOrDefault(l => l.Title.Contains("Variables"));
        var methodsLesson = lessons.FirstOrDefault(l => l.Title.Contains("Methods"));
        var classesLesson = lessons.FirstOrDefault(l => l.Title.Contains("Classes"));

        if (variablesLesson != null && methodsLesson != null)
        {
            variablesLesson.Id.Should().BeLessThan(methodsLesson.Id);
        }

        if (methodsLesson != null && classesLesson != null)
        {
            methodsLesson.Id.Should().BeLessThan(classesLesson.Id);
        }
    }

    [Fact]
    public void AllLessons_ShouldHaveReasonableContentLength()
    {
        // Arrange
        var allLessons = LessonContentService.CSharpBasicsLessons
            .Concat(LessonContentService.AspNetCoreLessons);

        // Act & Assert
        foreach (var lesson in allLessons)
        {
            // Content should be substantial but not excessively long
            lesson.Content.Length.Should().BeGreaterThan(500, "lessons should have substantial content");
            lesson.Content.Length.Should().BeLessThan(50000, "lessons should not be excessively long");
            
            lesson.Description.Length.Should().BeGreaterThan(20, "descriptions should be meaningful");
            lesson.Description.Length.Should().BeLessThan(500, "descriptions should be concise");
        }
    }
}