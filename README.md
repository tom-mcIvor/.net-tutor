# .NET Tutor — Learn .NET by Building with .NET

## MVP

The **.NET Tutor App** is a learning tool built with **.NET** (backend) and **React + TypeScript** (frontend). It is designed to present structured, interactive content inspired by the official .NET documentation, with the goal of helping me deepen my understanding of the .NET Framework.

**The MVP will include:**

1. **Side Navigation Bar**  
   - Persistent left-hand side nav with at least **6 clickable tabs**, e.g.:  
     1. Introduction to .NET  
     2. C# Basics  
     3. ASP.NET Core Overview  
     4. Entity Framework Core  
     5. .NET Libraries & Tools  
     6. Advanced Topics  

2. **Content Display**  
   - Each tab loads a dedicated page with text, code snippets, and examples.  
   - Syntax highlighting for code samples.  

3. **Basic User Authentication**  
   - Sign up, log in, and log out with email/password.  
   - JWT or cookie-based auth.  

4. **Responsive Design**  
   - Optimized layout for desktop, tablet, and mobile viewing.  

5. **Basic Search Functionality**  
   - Search bar to find topics or keywords within the tutorial content.  

6. **Error & Loading States**  
   - Graceful error handling for missing content or API failures.  
   - Loading spinners or placeholders during data fetches.  

7. **AWS Deployment**  
   - Fully deployed and accessible via a public AWS endpoint (using AWS Elastic Beanstalk, EC2, or Amplify).  
   - Basic CI/CD pipeline for automated deployments.  

8. **Stripe Integration**  
   - Basic payment flow for unlocking premium content.  
   - Test mode enabled for development.  

---

## Stretch Goals

- **Interactive Code Playground** — Allow users to run .NET code snippets directly in the browser.  
- **Progress Tracking** — Track which lessons/topics have been completed.  
- **Quizzes & Challenges** — Add short tests after each section to reinforce learning.  
- **Dark Mode** — Optional theme toggle for user comfort.  
- **User Profiles** — Store bookmarks, progress, and payment history.  
- **Admin Dashboard** — Manage content, view user metrics, and handle Stripe transactions from a secure interface.  
- **Offline Mode** — Cache lessons for reading without internet.  
- **Multi-language Support** — Offer the tutorial in multiple languages for broader accessibility.  




 repository is a hands-on learning project that teaches modern .NET development by using .NET itself. You will build and run an ASP.NET Core Web API (backend) and interact with it from a simple frontend. The emphasis is on understanding .NET fundamentals, conventions, and developer workflow.

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
