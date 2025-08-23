# Setting up Entity Framework Core

## Prerequisites

Before we start, make sure you have:
- ✅ .NET 8 SDK installed
- ✅ A code editor (Visual Studio, VS Code, or Rider)
- ✅ Basic understanding of C# classes and properties

## Step 1: Create a New Project

Let's create a new console application to learn EF Core:

```bash
# Create a new console application
dotnet new console -n EFCoreDemo
cd EFCoreDemo
```

## Step 2: Install EF Core Packages

EF Core is modular - you install only what you need:

### Core Package (Always Required)
```bash
dotnet add package Microsoft.EntityFrameworkCore
```

### Database Provider (Choose One)

#### For SQL Server:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

#### For SQLite (Great for learning):
```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

#### For PostgreSQL:
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

### Design-Time Tools (For Migrations)
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Install EF Core Tools Globally
```bash
dotnet tool install --global dotnet-ef
```

Verify installation:
```bash
dotnet ef
```

## Step 3: Create Your First Entity

Create a `Models` folder and add your first entity:

```csharp
// Models/Blog.cs
using System.ComponentModel.DataAnnotations;

namespace EFCoreDemo.Models
{
    public class Blog
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublished { get; set; }
        
        // Navigation property for related posts
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
```

```csharp
// Models/Post.cs
using System.ComponentModel.DataAnnotations;

namespace EFCoreDemo.Models
{
    public class Post
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        
        // Foreign key
        public int BlogId { get; set; }
        
        // Navigation property
        public Blog Blog { get; set; }
    }
}
```

## Step 4: Create Your DbContext

The DbContext is the heart of EF Core:

```csharp
// Data/BloggingContext.cs
using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Models;

namespace EFCoreDemo.Data
{
    public class BloggingContext : DbContext
    {
        // DbSet properties represent tables
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        
        // Configure the database connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Using SQLite for simplicity (file-based database)
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }
        
        // Optional: Configure entity relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-many relationship
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.BlogId);
        }
    }
}
```

## Step 5: Configure Connection Strings (Best Practice)

Instead of hardcoding the connection string, use configuration:

### Create appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=blogging.db"
  }
}
```

### Updated DbContext
```csharp
using Microsoft.Extensions.Configuration;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlite(connectionString);
    }
}
```

### Install Configuration Package
```bash
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
```

## Step 6: Test Your Setup

Update your `Program.cs`:

```csharp
using EFCoreDemo.Data;
using EFCoreDemo.Models;

Console.WriteLine("EF Core Demo Starting...");

using (var context = new BloggingContext())
{
    // Check if we can connect to the database
    var canConnect = context.Database.CanConnect();
    Console.WriteLine($"Can connect to database: {canConnect}");
    
    // Get the connection string being used
    Console.WriteLine($"Connection string: {context.Database.GetConnectionString()}");
}

Console.WriteLine("Setup complete! Ready to create migrations.");
```

## Step 7: Project Structure

Your project should now look like this:

```
EFCoreDemo/
├── Models/
│   ├── Blog.cs
│   └── Post.cs
├── Data/
│   └── BloggingContext.cs
├── appsettings.json
├── Program.cs
└── EFCoreDemo.csproj
```

## Common Setup Patterns

### Dependency Injection (Web Applications)
```csharp
// In ASP.NET Core Startup or Program.cs
services.AddDbContext<BloggingContext>(options =>
    options.UseSqlite(connectionString));
```

### Multiple Database Providers
```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    {
        optionsBuilder.UseSqlite("Data Source=dev.db");
    }
    else
    {
        optionsBuilder.UseSqlServer(productionConnectionString);
    }
}
```

## Testing Your Setup

Run your application:
```bash
dotnet run
```

You should see:
```
EF Core Demo Starting...
Can connect to database: False
Connection string: Data Source=blogging.db
Setup complete! Ready to create migrations.
```

Don't worry that it shows "False" - we haven't created the database yet! That's what we'll do with migrations in the next lesson.

## Common Setup Issues

### Issue: Package Not Found
**Solution:** Make sure you're using compatible versions:
```bash
dotnet list package  # Check installed versions
```

### Issue: Connection String Problems
**Solution:** Verify your connection string format:
- SQLite: `"Data Source=database.db"`
- SQL Server: `"Server=.;Database=MyDb;Trusted_Connection=true;"`

### Issue: Tools Not Working
**Solution:** Reinstall EF Core tools:
```bash
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
```

## Quick Setup Checklist

- ✅ Project created
- ✅ EF Core packages installed
- ✅ Entity classes created
- ✅ DbContext configured
- ✅ Connection string set up
- ✅ EF Core tools installed

## What's Next?

In the next lesson, you'll learn about:
- Creating entity classes with proper conventions
- Understanding primary keys and foreign keys
- Using data annotations vs Fluent API
- Entity relationships and navigation properties

Great job! You now have a solid EF Core foundation. Let's dive into creating your data models!