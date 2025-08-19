# .NET Tutor — Learn .NET with My .NET app

🌐 **Live Demo:** [net-tutor-tommcivors-projects.vercel.app](https://net-tutor-tommcivors-projects.vercel.app)

---

## 🚀 MVP (First Release Goals)

The **.NET Tutor App** is a learning platform built with **.NET** (backend) and **React + TypeScript** (frontend).
It delivers interactive, structured content inspired by official .NET documentation, designed to deepen understanding of the .NET Framework.

**The MVP will include:**

- ✅ **Structured Side Navigation**
   - Persistent left-hand side nav with at least **6 clickable tabs**:
     1. Introduction to .NET
     2. C# Basics
     3. ASP.NET Core Overview
     4. Entity Framework Core
     5. .NET Libraries & Tools
     6. Advanced Topics

- ✅ **Interactive Content Display**
   - Each tab loads a dedicated page with text, code snippets, and examples.
   - Syntax highlighting for code samples.

- ✅ **Create Comprehensive Learning Material**
   - Detailed educational content for each navigation tab:
     1. **Introduction to .NET** - Platform overview, key concepts, Hello World example
     2. **C# Basics** - Variables, data types, control structures, OOP principles
     3. **ASP.NET Core Overview** - Web development, MVC pattern, API creation
     4. **Entity Framework Core** - Database access, ORM patterns, migrations
     5. **.NET Libraries & Tools** - NuGet packages, CLI tools, ecosystem
     6. **Advanced Topics** - Microservices, testing, deployment strategies

- ✅ **Responsive Design**
   - Optimized for desktop, tablet, and mobile viewing.

- ✅ **Basic Search Functionality**
   - Search bar to find topics or keywords within the tutorial content.

- ✅ **Error & Loading States**
   - Graceful error handling for missing content or API failures.
   - Loading spinners/placeholders while fetching data.

---

## 🌟 Stretch Goals

- [ ] **AWS Deployment** — Public cloud hosting with CI/CD pipeline for automated deployments (Elastic Beanstalk, EC2, or Amplify).
- [ ] **Stripe Integration** — Payment flow for unlocking premium content (test mode during development).
- [ ] **Interactive Code Playground** — Run .NET code snippets in the browser.
- ✅ **Progress Tracking** — Track completed lessons and topics.
- ✅ **Basic User Authentication** — Sign up, log in, and log out with email/password using JWT authentication.
- [ ] **Quizzes & Challenges** — Short tests after sections to reinforce learning.
- ✅ **Dark Mode** — Theme toggle for user comfort.
- [ ] **User Profiles** — Store bookmarks, progress, and payment history.
- [ ] **Admin Dashboard** — Manage content, metrics, and Stripe transactions.
- [ ] **Offline Mode** — Cache lessons for offline reading.
- [ ] **Multi-language Support** — Tutorials in multiple languages.

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
