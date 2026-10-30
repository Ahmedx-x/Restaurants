# Restaurants API

A RESTful ASP.NET Core 8 Web API designed for managing restaurants and their associated dishes. It features user accounts and roles, claim-based policies, per-resource ownership authorization, CQRS-style request handling, and Docker-based containerization.

---

## 🚀 Features

- **CQRS Request Handling:** Implemented using MediatR 12 for clean separation of commands and queries.
- **Robust Validation & Mapping:** Input validation using FluentValidation and object mapping with AutoMapper.
- **Multi-Layered Security & Authorization:**
  - ASP.NET Core Identity for user accounts, registration, login, and token generation.
  - Role-based access control (`Admin`, `Owner`, `User`).
  - Policy-based authorization, including claim-based nationality checks and minimum age requirements.
  - Resource-level ownership authorization, ensuring users can only modify or delete restaurants and dishes they own, unless they are administrators.
- **Data Management & Persistence:** Entity Framework Core 8 with SQL Server, database migrations, and automatic data seeding.
- **Pagination, Filtering & Sorting:** Server-side search, pagination using `PagedResult<T>`, and configurable sorting rules.
- **Observability & Error Handling:** Custom middleware for centralized exception handling and request-timing logging, backed by Serilog with console and rolling file sinks.
- **API Documentation:** Interactive Swagger/OpenAPI documentation with Bearer authentication support in development.
- **Automated Testing:** 34 passing unit and integration-style tests using xUnit, Moq, FluentAssertions, and `WebApplicationFactory`.
- **Docker Containerization:** The API and SQL Server run in separate Docker containers orchestrated with Docker Compose.
- **Persistent Database Storage:** Docker volumes preserve SQL Server data across container restarts and recreation.
- **Containerized Database Migrations:** EF Core migrations can be applied to the SQL Server instance running inside Docker.
- **Environment-Based Configuration:** The API connects to SQL Server through Docker's internal network using service-based connection settings.

---

## 🐳 Docker Setup

The project uses Docker Compose to run the ASP.NET Core API and SQL Server as separate services.

### Architecture

```text
                     Client / Browser
                            |
                            v
                    Swagger / REST API
                     localhost:5000
                            |
                            v
                 +----------------------+
                 |   Restaurants API    |
                 |  ASP.NET Core 8      |
                 |  Container: API      |
                 +----------------------+
                            |
                    Docker Network
                            |
                            v
                 +----------------------+
                 |      SQL Server      |
                 |  Container: SQL      |
                 +----------------------+
                            |
                            v
                    Persistent Volume
```

### Prerequisites

Install the following tools before running the project:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) — useful for local development and running EF Core CLI commands.
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) — required to build and run the containers.

Make sure Docker Desktop is running before executing the commands below.

### Run the Application

1. Clone the repository:

   ```bash
   git clone https://github.com/Ahmedx-x/Restaurants
   cd Restaurants
   ```

2. Build the API image and start the containers:

   ```bash
   docker compose up -d --build
   ```

3. Check the container status:

   ```bash
   docker compose ps
   ```

4. View the API logs:

   ```bash
   docker compose logs -f restaurants-api
   ```

5. Open Swagger in your browser:

   **http://localhost:5000/swagger**

   The API is exposed on host port `5000`, mapped to port `8080` inside the API container in the current Docker configuration.

### Docker Compose Commands

**Start the application:**

```bash
docker compose up -d
```

**Rebuild and start after code changes:**

```bash
docker compose up -d --build
```

**Check running containers:**

```bash
docker compose ps
```

**Follow application logs:**

```bash
docker compose logs -f restaurants-api
```

**Stop and remove the containers:**

```bash
docker compose down
```

Stopping the application with `docker compose down` does not normally remove named volumes, so database data can persist when the containers are recreated.

> **Warning:** Avoid `docker compose down -v` if you want to preserve the database volume. The `-v` option removes Compose-managed volumes.

---

## 🗄️ Database & Migrations

The application uses SQL Server running in a Docker container.

Inside the Docker network, the API connects to SQL Server through its Compose service name, for example:

```text
Server=sqlserver,1433;Database=RestaurantsDb;...
```

The hostname `sqlserver` is resolved through Docker Compose's internal network. When connecting to the same database from your host machine, use the published SQL Server port, such as `localhost,1433`.

### Apply EF Core Migrations

To apply migrations to the SQL Server instance running inside Docker, run the following command from the repository root in PowerShell:

```powershell
dotnet ef database update `
  --project src/Restaurants.Infrastructure `
  --startup-project src/Restaurants.API `
  --connection "Server=localhost,1433;Database=RestaurantsDb;User Id=sa;Password=<YOUR_SA_PASSWORD>;Encrypt=True;TrustServerCertificate=True;"
```

Replace `<YOUR_SA_PASSWORD>` with the SQL Server password configured for your local Docker environment.

**Important:** The explicit connection string directs the EF Core CLI to the Docker-hosted database through the published host port. Without it, your local configuration may point to a different SQL Server instance.

The API also runs the configured data seeder at startup to initialize the required sample data and roles. The seeder should be safe to run repeatedly without creating duplicate seed records.

---

## 🏗️ Architecture

The solution follows a layered Clean Architecture / N-Tier hybrid layout across four primary projects:

- **`Restaurants.Domain`:** Core enterprise layer containing entities (`Restaurant`, `Dish`, `Address`, `User`), domain exceptions, repository interfaces, and authorization service contracts.
- **`Restaurants.Application`:** Business logic layer holding CQRS commands, queries, handlers, DTOs, FluentValidation validators, and AutoMapper profiles.
- **`Restaurants.Infrastructure`:** Data access and external services layer containing `RestaurantsDbContext`, EF Core SQL Server migrations, repository implementations, seeders, and security/authorization handlers.
- **`Restaurants.API`:** Presentation entry point containing API controllers, custom middleware, dependency injection, and application configuration.

---

## 🛠️ Technology Stack

- **Framework:** ASP.NET Core 8 (`net8.0`)
- **Database & ORM:** SQL Server, Entity Framework Core 8
- **Authentication & Identity:** ASP.NET Core Identity
- **CQRS & Mediator:** MediatR 12.2.0
- **Validation & Mapping:** FluentValidation 11.3.1, AutoMapper 12.0.1
- **Logging:** Serilog 8.0.0
- **API Documentation:** Swashbuckle 8.0.0 (Swagger)
- **Testing:** xUnit 2.5.3, Moq 4.21.0, FluentAssertions 8.11.0, `Microsoft.AspNetCore.Mvc.Testing`
- **Containerization:** Docker, Docker Compose
- **Database Container:** SQL Server running in a dedicated container
- **Database Persistence:** Docker volumes

---

## 📁 Project Structure

```text
Restaurants/
├── Restaurants.slnx
├── docker-compose.yml
├── Dockerfile
├── .dockerignore
├── src/
│   ├── Restaurants.Domain/
│   ├── Restaurants.Application/
│   ├── Restaurants.Infrastructure/
│   │   └── Migrations/
│   └── Restaurants.API/
│       ├── Controllers/
│       ├── Middlewares/
│       └── Restaurants.API.http
└── tests/
    ├── Restaurants.Application.Tests/
    ├── Restaurants.Infrastructure.Tests/
    └── Restaurants.API.Tests/
```

---

## 🧪 Running Tests

Run the test suite from the repository root:

```bash
dotnet test
```

The tests cover application behavior and API functionality using xUnit, Moq, FluentAssertions, and `WebApplicationFactory`.

---

## 🔐 Security Notes

- Do not commit real database passwords, API keys, or other secrets to source control.
- Use environment variables or a secrets-management solution to configure credentials.
- Keep production secrets separate from development configuration.
- Restrict Swagger exposure appropriately outside development environments.
- Configure SQL Server credentials securely before deploying the application.
- Ensure ownership and role-based authorization are tested for unauthorized access attempts.

---

## 📌 Future Improvements

- Add a GitHub Actions CI pipeline for automated builds and tests.
- Add a container image publishing workflow.
- Introduce a deployment pipeline for a cloud hosting environment.
- Add structured health checks for the API and database.
- Improve production observability and centralized log collection.