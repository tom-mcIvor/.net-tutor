# .NET Tutor — Learn .NET with My .NET app

---

## 🚀 MVP (First Release Goals)

The **.NET Tutor App** is a learning tool built with **.NET** (backend) and **React + TypeScript** (frontend).  
It presents structured, interactive content inspired by the official .NET documentation, with the goal of deepening my understanding of the .NET Framework.

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
   - JWT or cookie-based authentication.  

4. **Responsive Design**  
   - Optimized for desktop, tablet, and mobile viewing.  

5. **Basic Search Functionality**  
   - Search bar to find topics or keywords within the tutorial content.  

6. **Error & Loading States**  
   - Graceful error handling for missing content or API failures.  
   - Loading spinners/placeholders while fetching data.  

7. **AWS Deployment**  
   - Publicly accessible deployment on AWS (Elastic Beanstalk, EC2, or Amplify).  
   - Basic CI/CD pipeline for automated deployments.  

8. **Stripe Integration**  
   - Basic payment flow for unlocking premium content.  
   - Test mode enabled during development.  

---

## 🌟 Stretch Goals

- **Interactive Code Playground** — Run .NET code snippets in the browser.  
- **Progress Tracking** — Track completed lessons/topics.  
- **Quizzes & Challenges** — Short tests after sections to reinforce learning.  
- **Dark Mode** — Theme toggle for user comfort.  
- **User Profiles** — Store bookmarks, progress, and payment history.  
- **Admin Dashboard** — Manage content, metrics, and Stripe transactions.  
- **Offline Mode** — Cache lessons for offline reading.  
- **Multi-language Support** — Tutorials in multiple languages.

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

