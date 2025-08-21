# Welcome to ASP.NET Core

## What Can You Build with ASP.NET Core?

- **Web Applications** (MVC, Razor Pages)
- **Web APIs** (RESTful services)
- **Real-time Applications** (SignalR)
- **Microservices** (Containerized applications)
- **Cloud Applications** (Azure, AWS, Google Cloud)

## What is ASP.NET Core?

ASP.NET Core is a modern, cross-platform web framework developed by Microsoft. It's part of the .NET ecosystem.

## Why Learn ASP.NET Core?

✅ **Cross-Platform**: Runs on Windows, macOS, and Linux
✅ **High Performance**: One of the fastest web frameworks available
✅ **Modern Architecture**: Built-in dependency injection, configuration, and logging
✅ **Flexible Deployment**: Deploy to cloud, on-premises, or containers
✅ **Strong Ecosystem**: Rich package ecosystem via NuGet

## Your First ASP.NET Core Application

```csharp
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapGet("/", () => "Hello, ASP.NET Core!");
app.MapGet("/api/hello", () => new { Message = "Welcome to ASP.NET Core!" });

app.Run();
```

**What this code does:**
- `WebApplication.CreateBuilder()` - Creates the application builder
- `AddControllers()` - Adds MVC controller services
- `MapGet()` - Maps HTTP GET requests to endpoints
- `app.Run()` - Starts the web server

## Key Concepts to Remember

1. **Middleware Pipeline**: Requests flow through a series of middleware components
2. **Dependency Injection**: Built-in DI container manages object lifetimes
3. **Configuration**: Flexible configuration system with multiple sources
4. **Routing**: Maps URLs to controller actions or minimal API endpoints

## Next Steps

In the next lesson, we'll explore MVC fundamentals - the foundation of building web applications with ASP.NET Core!