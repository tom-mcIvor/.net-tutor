using Microsoft.AspNetCore.Mvc;
using DotNetTutor.Api.Services;
using DotNetTutor.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DotNetTutor.Api.Controllers;

[ApiController]
[Route("api/lessons")]
public class LessonsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LessonsController(ApplicationDbContext context)
    {
        _context = context;
    }
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

    [HttpGet("aspnetcore")]
    public IActionResult GetAspNetCoreLessons() => Ok(LessonContentService.AspNetCoreLessons);

    [HttpGet("aspnetcore/{id:int}")]
    public IActionResult GetAspNetCoreLessonById(int id)
    {
        var lesson = Array.Find(LessonContentService.AspNetCoreLessons, l => l.Id == id);
        return lesson is null ? NotFound() : Ok(lesson);
    }

    [HttpGet("aspnetcore/count")]
    public IActionResult GetAspNetCoreLessonCount() => Ok(new { count = LessonContentService.AspNetCoreLessons.Length });

    [HttpGet("aspnetcore/topics")]
    public IActionResult GetAspNetCoreTopics()
    {
        var topics = LessonContentService.AspNetCoreLessons
            .Select(l => new { l.Id, l.Title, l.Description })
            .ToArray();
        return Ok(topics);
    }

    [HttpGet("database")]
    public async Task<IActionResult> GetFromDatabase()
    {
        var lessons = await _context.Lessons.ToListAsync();
        return Ok(lessons);
    }
}

public class LessonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
