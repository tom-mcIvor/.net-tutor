# .NET Tutor — Learn .NET by Building with .NET

This repository is a hands-on learning project that teaches modern .NET development by using .NET itself. You will build and run an ASP.NET Core Web API (backend) and interact with it from a simple frontend. The emphasis is on understanding .NET fundamentals, conventions, and developer workflow.

## Why This Project

- Learn .NET 8 with real code, not snippets.
- Understand ASP.NET Core API design, configuration, DI, middleware, and Swagger/OpenAPI.
- Practice the .NET CLI workflow: new, build, run, publish, test.
- See how a frontend consumes a typed .NET API during local development.

## Tech Overview (.NET Focus)

- .NET 8 SDK
- ASP.NET Core Web API with Controllers
- Dependency Injection and minimal hosting model
- Configuration via appsettings.json and environments
- Swagger (Swashbuckle) for interactive API exploration

## Repository Structure

- backend/
  - DotNetTutor.Api/ — ASP.NET Core Web API project (primary learning focus)
- frontend/
  - Vite + React app that calls the .NET API (supporting role for end-to-end demo)
- dot-net-tutor.sln — Solution file for working with the backend in IDEs like Rider or VS

For backend details, see [backend/README.md](backend/README.md).

## Prerequisites

- .NET 8 SDK
- Node.js (only for running the optional frontend during local development)

Verify .NET:
- dotnet --version

## .NET Learning Path in This Repo

1) Explore the API project layout
- Program.cs: app bootstrap (builder, services, middleware, Swagger)
- Controllers/LessonsController.cs: REST endpoints and routing
- Models/Lesson.cs: simple domain model
- appsettings.json + appsettings.Development.json: configuration binding

2) Run the API
- cd ./backend/DotNetTutor.Api
- dotnet run --launch-profile "http" --no-restore
- Navigate to Swagger: http://localhost:5000/swagger

3) Inspect and extend endpoints
- Add routes, validate input, return IActionResult, explore model binding.
- Experiment with DI: register services in Program.cs and inject into controllers.

4) Build and publish
- dotnet build
- dotnet publish -c Release -o out
- Review the published output for deployment.

5) Add tests (optional next step)
- dotnet new xunit -o ../DotNetTutor.Tests
- dotnet add ../DotNetTutor.Tests/DotNetTutor.Tests.csproj reference DotNetTutor.Api.csproj
- dotnet test

## Daily .NET CLI Workflow

- Restore (usually implicit with build/run): dotnet restore
- Build: dotnet build
- Run: dotnet run
- Test: dotnet test
- Publish: dotnet publish -c Release -o out
- New project templates: dotnet new --list

## Running the Full Stack (Optional)

- Start backend: http://localhost:5000
- Start frontend (from ./frontend):
  - npm install
  - npm run dev -- --host
- The frontend consumes the backend’s /api endpoints.

If ports differ, adjust frontend env at frontend/.env.local.

## Goals and Next Steps

- Add new endpoints and apply best practices (DTOs, validation, logging).
- Introduce persistence (EF Core) and migrations.
- Add integration/unit tests.
- Containerize with Docker after you understand the local flow.

## License

MIT