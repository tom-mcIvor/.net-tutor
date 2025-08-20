using Microsoft.AspNetCore.Mvc;
using DotNetTutor.Api.Services;

namespace DotNetTutor.Api.Controllers;

[ApiController]
[Route("api/lessons")]
public class LessonsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok(LessonContentService.CSharpBasicsLessons);

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var lesson = Array.Find(LessonContentService.CSharpBasicsLessons, l => l.Id == id);
        return lesson is null ? NotFound() : Ok(lesson);
    }

    [HttpGet("count")]
    public IActionResult GetLessonCount() => Ok(new { count = LessonContentService.CSharpBasicsLessons.Length });

    [HttpGet("topics")]
    public IActionResult GetTopics()
    {
        var topics = LessonContentService.CSharpBasicsLessons
            .Select(l => new { l.Id, l.Title, l.Description })
            .ToArray();
        return Ok(topics);
    }
}

public class LessonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
