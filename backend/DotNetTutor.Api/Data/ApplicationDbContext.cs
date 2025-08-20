using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DotNetTutor.Api.Models;

namespace DotNetTutor.Api.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Seed some lessons
        builder.Entity<Lesson>().HasData(
            new Lesson(1, "Introduction to .NET", "Overview of .NET and its components.", "Detailed content about .NET framework..."),
            new Lesson(2, "ASP.NET Core Basics", "Learn the basics of building web APIs.", "Detailed content about ASP.NET Core...")
        );
    }
}