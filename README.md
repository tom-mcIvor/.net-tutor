# .NET Tutor — Interactive .NET Learning Platform

🌐 **Live Demo:** [net-tutor-tommcivors-projects.vercel.app](https://net-tutor-tommcivors-projects.vercel.app)

---

## 🚀 MVP (First Release Goals) - ✅ COMPLETED

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

- ✅ **Interactive Content Display**
   - Each tab loads a dedicated page with rich content, code snippets, and examples.
   - Syntax highlighting for code samples with VS Code-style theming.
   - Interactive lesson cards with hover effects and navigation.

- ✅ **Rich Content System**
   - Comprehensive lesson content with markdown support and JSON metadata.
   - Individual lesson pages with detailed content and navigation.
   - Lesson metadata system for organized content management.

- ✅ **Interactive UI Components**
   - Animated lesson cards with hover effects and smooth transitions.
   - Progress indicators and responsive design elements.
   - Mobile-first design with collapsible navigation.

- ✅ **Comprehensive Learning Material**
   - Detailed educational content for navigation tabs:
     1. **Introduction to .NET** ✅ - Platform overview, key concepts, Hello World example, interactive search
     2. **C# Basics** ✅ - Variables, data types, control structures, OOP principles, interactive quizzes, coding challenges
     3. **ASP.NET Core Overview** ✅ - Web development, MVC pattern, API creation, comprehensive lessons
     4. **Entity Framework Core** 🚧 - Database access, ORM patterns, migrations (placeholder content)
     5. **.NET Libraries & Tools** 🚧 - NuGet packages, CLI tools, ecosystem (placeholder content)
     6. **Advanced Topics** 🚧 - Microservices, testing, deployment strategies (placeholder content)

- ✅ **Responsive Design**
   - Fully optimized for desktop, tablet, and mobile viewing.
   - Collapsible sidebar navigation for mobile devices.

- ✅ **Advanced Search Functionality**
   - Interactive search bar with real-time feedback.
   - Search suggestions and topic discovery.

- ✅ **Error & Loading States**
   - Comprehensive error handling for missing content or API failures.
   - Loading spinners and graceful fallbacks throughout the application.

- ✅ **Individual Lesson System**
   - Dedicated lesson pages with detailed content.
   - Lesson navigation and progress tracking.
   - JSON-based lesson metadata and markdown content.

- [ ] **Expanded Course Content**
   - Add more course cards for comprehensive coverage of each topic.
   - Flesh out learning tabs with structured, detailed content.
   - Create complete learning paths for each major .NET area.

- [ ] **Interactive Quiz System**
   - Add quizzes to reinforce learning after each section.
   - Multiple choice questions with instant feedback.
   - Progress tracking for quiz completion and scores.

- ✅ **AWS Cloud Deployment**
   - Production deployment on AWS EC2 with Docker containers
   - CloudFormation infrastructure as code with VPC, security groups, and monitoring
   - Automated deployment scripts with health checks and rollback capabilities
   - Live at: http://3.212.142.135

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
- ✅ **Mobile-First Design** — Fully responsive design optimized for all device sizes.
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
