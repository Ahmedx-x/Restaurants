# Restaurants API

A RESTful ASP.NET Core 8 Web API designed for managing restaurants and their associated dishes. It features user accounts and roles, claim-based policies, per-resource ownership authorization, and CQRS-style request handling.

---

## 🚀 Features

* **CQRS Request Handling:** Implemented using MediatR 12 for clean separation of commands and queries.
* **Robust Validation & Mapping:** Input validation enforced via FluentValidation (hooked into MVC model validation) and object mapping handled by AutoMapper[cite: 1].
* **Multi-Layered Security & Authorization:** 
  * ASP.NET Core Identity for user accounts, registration, login, and token generation[cite: 1].
  * Role-based access control (`Admin`, `Owner`, `User`).
  * Policy-based authorization (e.g., claim-based nationality checks and minimum age requirements).
  * Resource-level ownership authorization ensuring users can only modify or delete restaurants/dishes they own (or if they are admins).
* **Data Management & Persistence:** Built with Entity Framework Core 8 and SQL Server, incorporating migrations and an automatic data seeder.
* **Pagination, Filtering, & Sorting:** Server-side search phrases, pagination (`PagedResult<T>`), and allowed sorting rules.
* **Observability & Error Handling:** Custom middleware for centralized error handling (mapping custom domain exceptions to proper HTTP status codes) and request-timing logging, backed by Serilog (console and rolling file sinks).
* **API Documentation:** Interactive Swagger/OpenAPI documentation configured with a Bearer security scheme (enabled in development environments).
* **Automated Tests:** Comprehensive unit and integration-ish test suite consisting of 34 passing tests built with xUnit, Moq, FluentAssertions, and `WebApplicationFactory`.

---

## 🏗️ Architecture

The solution follows a layered Clean Architecture / N-Tier hybrid layout across four primary projects[cite: 1]:

* **`Restaurants.Domain`:** Core enterprise layer containing entities (`Restaurant`, `Dish`, `Address`, `User`), domain exceptions, repository interfaces, and authorization service contracts.
* **`Restaurants.Application`:** Business logic layer holding CQRS commands, queries, handlers, DTOs, FluentValidation validators, and AutoMapper profiles.
* **`Restaurants.Infrastructure`:** Data access and external services layer containing the `RestaurantsDbContext`, EF Core SQL Server migrations, repository implementations, seeders, and security/authorization handlers.
* **`Restaurants.API`:** Presentation entry point containing API controllers, custom middleware, presentation dependency injection, and configuration files.

---

## 🛠️ Technology Stack

* **Framework:** ASP.NET Core 8 (`net8.0`)
* **Database & ORM:** SQL Server, Entity Framework Core 8
* **Authentication & Identity:** ASP.NET Core Identity
* **CQRS & Mediator:** MediatR 12.2.0
* **Validation & Mapping:** FluentValidation 11.3.1, AutoMapper 12.0.1
* **Logging:** Serilog 8.0.0
* **API Documentation:** Swashbuckle 8.0.0 (Swagger)
* **Testing:** xUnit 2.5.3, Moq 4.21.0, FluentAssertions 8.11.0, `Microsoft.AspNetCore.Mvc.Testing`

---

## 📁 Project Structure

```text
Restaurants/
├─ Restaurants.slnx
├─ src/
│  ├─ Restaurants.Domain/
│  ├─ Restaurants.Application/
│  ├─ Restaurants.Infrastructure/
│     └─ Migrations/ (EF Core SQL Server migrations)
│  └─ Restaurants.API/
│     ├─ Controllers/
│     ├─ Middlewares/
│     └─ Restaurants.API.http
└─ tests/
   ├─ Restaurants.Application.Tests/
   ├─ Restaurants.Infrastructure.Tests/
   └─ Restaurants.API.Tests/
