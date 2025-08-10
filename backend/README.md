# DotNetTutor Backend (ASP.NET Core Web API)

This is the backend API for DotNetTutor, built with ASP.NET Core (.NET 8). It exposes endpoints for lessons and serves as the data provider for the Vite/React frontend.

## Tech Stack

- .NET 8 ASP.NET Core Web API
- Minimal setup with Controllers
- Swashbuckle/Swagger for API exploration
- JSON configuration via appsettings.json

## Project Structure

- backend/DotNetTutor.Api/
  - Program.cs: Web application setup, services, middleware, Swagger.
  - Controllers/LessonsController.cs: Endpoints for lessons.
  - Models/Lesson.cs: Lesson domain model.
  - appsettings.json, appsettings.Development.json: Configuration.
  - Properties/launchSettings.json: Profiles for local running.

## Requirements

- .NET 8 SDK
- Linux/macOS/Windows supported

Verify installation:
- dotnet --version

## Running Locally

From repository root:
- cd ./backend/DotNetTutor.Api
- dotnet run --launch-profile "http" --no-restore

By default this starts on http://localhost:5000 (HTTP). Swagger UI is available at:
- http://localhost:5000/swagger

If HTTPS profile is used (via launch settings), ports may differ (e.g., 5001).

## API Overview

Base URL (dev): http://localhost:5000

- GET /api/lessons
  - Returns a list of lessons.
- GET /api/lessons/{id}
  - Returns a specific lesson by id.

Explore via Swagger:
- http://localhost:5000/swagger

## Configuration

- appsettings.json for shared config.
- appsettings.Development.json for local overrides.
- Environment is typically "Development" when running locally.

## Building

From backend/DotNetTutor.Api:
- dotnet build

## Testing (if/when added)

- This repo currently has no test project. A typical structure would be backend/DotNetTutor.Tests with xUnit. You can add one with:
  - dotnet new xunit -o ../DotNetTutor.Tests
  - dotnet add ../DotNetTutor.Tests/DotNetTutor.Tests.csproj reference DotNetTutor.Api.csproj

## Common Issues

- Port conflicts: If 5000 is occupied, ASP.NET may pick another port; check console output for the actual URLs.
- Swagger not loading: Ensure Development environment or Swagger enabled in Program.cs for production if desired.

## Frontend Integration

The React/Vite frontend calls this API. Ensure the backend is running before starting the frontend dev server.

Default flows:
- Backend: http://localhost:5000
- Frontend dev server: http://localhost:5173

If you change backend URL/port, update the frontend environment (frontend/.env.local) accordingly.

## Deployment

For containerization or hosting, publish:
- dotnet publish -c Release -o out
Then deploy the contents of out/ to your hosting environment or build a Docker image around it.
