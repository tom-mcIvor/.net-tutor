# .NET Tutor — Interactive .NET Learning Platform

🌐 **Live Demo of the frontend only:** [net-tutor-tommcivors-projects.vercel.app](https://net-tutor-tommcivors-projects.vercel.app)

---

## 🚀 MVP (First Release Goals) 

The **.NET Tutor App** is a learning platform built with **.NET** (backend) and **React + TypeScript** (frontend).
It delivers interactive, structured content inspired by official .NET documentation, designed to deepen understanding of the .NET Framework.

**The MVP includes:**

- ✅ **Structured Side Navigation**
   - Persistent left-hand side nav with **7 clickable tabs**:
     1. Introduction to .NET
     2. C# Basics
     3. ASP.NET Core Overview
     4. Entity Framework Core
     5. .NET Libraries & Tools
     6. Advanced Topics
     7. Profile (User Management)
    
- ✅ ** Learning Material**
   - Detailed educational content for navigation tabs:
     1. **Introduction to .NET** ✅ - Platform overview, key concepts, Hello World example, interactive search
     2. **C# Basics** ✅ - Variables, data types, control structures, OOP principles, interactive quizzes, coding challenges
     3. **ASP.NET Core Overview** ✅ - Web development, MVC pattern, API creation, comprehensive lessons
     4. **Entity Framework Core** 🚧 - Database access, ORM patterns, migrations (placeholder content)
     5. **.NET Libraries & Tools** 🚧 - NuGet packages, CLI tools, ecosystem (placeholder content)
     6. **Advanced Topics** 🚧 - Microservices, testing, deployment strategies (placeholder content)

- ✅ **Interactive UI Components**
   - Animated lesson cards with hover effects and smooth transitions.
   - Progress indicators and responsive design elements.
   - Mobile-first design with collapsible navigation.

- ✅ **Feedback footer incorporated into the database**
   -  responsive feedback footer.
 
- ✅ **AWS Cloud Deployment**
   - Production deployment on AWS EC2 with Docker containers
   - CloudFormation infrastructure as code with VPC, security groups, and monitoring
   - Automated deployment scripts with health checks and rollback capabilities
   - Live at: [Domain configured separately]

- [ ] **Expanded Course Content**
   - Add more course cards for comprehensive coverage of each topic.
   - Flesh out learning tabs with structured, detailed content.
   - Create complete learning paths for each major .NET area.

- [ ] **Interactive Quiz System**
   - Add quizzes to reinforce learning after each section.
   - Multiple choice questions with instant feedback.
   - Progress tracking for quiz completion and scores.

---

## 🌟 Stretch Goals

- [ ] **Stripe Integration** — Payment flow for unlocking premium content (test mode during development).
- [ ] **Interactive Code Playground** — Run .NET code snippets in the browser.
- ✅ **Progress Tracking** — Track completed lessons and topics with persistent storage.
- ✅ **User Authentication System** — Complete sign up, log in, and log out with email/password using JWT authentication.
- ✅ **Interactive Quizzes & Challenges** — Multiple choice quizzes and coding challenges with instant feedback.
- ✅ **Dark Mode** — Theme toggle for user comfort with system preference detection.
- ✅ **User Profiles** — Dedicated profile page with progress tracking and time spent analytics.
- ✅ **Feedback System** — User feedback collection and submission system.
- [ ] **Admin Dashboard** — Manage content, metrics, and user analytics.
- [ ] **Offline Mode** — Cache lessons for offline reading.
- [ ] **Multi-language Support** — Tutorials in multiple languages.

- ✅ **Advanced Navigation** — Hamburger menu, breadcrumbs, and smooth transitions.
- ✅ **Code Syntax Highlighting** — VS Code-style syntax highlighting for all code examples.
- ✅ **Learning Analytics** — Time tracking, completion rates, and progress visualization.
- ✅ **API Architecture** — RESTful API with proper error handling and validation.
- ✅ **Database Integration** — Entity Framework Core with SQLite for development.

---

## 📌 Why This Project

- Learn **.NET 8** with real code, not just snippets.
- Understand ASP.NET Core API design, configuration, DI, middleware, and Swagger/OpenAPI.
- Practice the **.NET CLI workflow**: `new`, `build`, `run`, `publish`, `test`.
- See how a frontend consumes a typed .NET API during development.

---

## 🛠 Tech Overview

**Backend (Primary Focus)**  
- .NET 8 SDK  
- ASP.NET Core Web API with Controllers  
- Dependency Injection (DI)  
- Configuration via `appsettings.json` and environments  
- Swagger (Swashbuckle) for API exploration

**Frontend (Support Role)**  
- React + TypeScript (Vite)  
- Calls the .NET API for end-to-end demo

---

## 📂 Repository Structure

```
.
├── backend/                    # .NET 8 Web API
│   ├── DotNetTutor.Api/       # Main API project
│   │   ├── Controllers/       # API controllers
│   │   ├── Data/             # Database context & configuration
│   │   ├── Lessons/          # Lesson content & metadata
│   │   ├── Models/           # Domain models & DTOs
│   │   ├── Services/         # Business logic services
│   │   └── Migrations/       # Entity Framework migrations
│   └── DotNetTutor.Tests/    # Unit & integration tests
│
├── frontend/                  # React + TypeScript
│   ├── src/
│   │   ├── components/       # Reusable UI components
│   │   ├── pages/           # Route-specific page components
│   │   ├── contexts/        # React context providers
│   │   ├── api/            # API client & service layer
│   │   └── types/          # TypeScript type definitions
│   ├── public/             # Static assets
│   └── dist/              # Production build output
│
├── deployment/               # AWS deployment scripts
│   ├── deploy.sh            # Main deployment automation
│   ├── cloudformation-template.yaml  # Infrastructure as code
│   └── NEW_ACCOUNT_DEPLOYMENT.md    # Deployment documentation
│
├── nginx/                   # Reverse proxy configuration
│   └── nginx.conf          # Production nginx config
│
├── .github/workflows/       # CI/CD pipeline
├── docker-compose.yml       # Local development setup
└── dot-net-tutor.sln       # .NET solution file
```
