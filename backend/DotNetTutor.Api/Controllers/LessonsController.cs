using Microsoft.AspNetCore.Mvc;

namespace DotNetTutor.Api.Controllers;

[ApiController]
[Route("api/lessons")]
public class LessonsController : ControllerBase
{
    private static readonly LessonDto[] Lessons = new[]
    {
        new LessonDto { Id = 1, Title = "Introduction to .NET", Description = "Overview of .NET and its components.", Content = "The .NET platform is a free, cross-platform, open-source developer platform for building many different types of applications..." },
        new LessonDto { Id = 2, Title = "ASP.NET Core Basics", Description = "Learn the basics of building web APIs.", Content = "ASP.NET Core is a cross-platform framework for building modern cloud-based, internet-connected applications..." }
    };

    [HttpGet]
    public IActionResult GetAll() => Ok(Lessons);

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var lesson = Array.Find(Lessons, l => l.Id == id);
        return lesson is null ? NotFound() : Ok(lesson);
    }
}

public class LessonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
