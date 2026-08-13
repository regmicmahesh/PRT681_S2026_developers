# JobBoard.Api backend

.NET 10 Web API built with Clean Architecture and Domain-Driven Design.

## Layers

```
src/
  JobBoard.Domain          Entities, value objects, domain events, repository interfaces. No dependencies.
  JobBoard.Application      Use cases (CQRS commands/queries via MediatR), validators. Depends on Domain.
  JobBoard.Persistence      EF Core DbContext, entity configurations, repository implementations, migrations.
  JobBoard.Infrastructure   Cross-cutting concerns (email notifications) reacting to domain events.
  JobBoard.Api              ASP.NET Core Web API composition root: controllers, DI wiring, Scalar docs.
tests/
  JobBoard.Domain.Tests     Unit tests for the Domain layer.
```

Dependencies only point inward: `Api` -> `Application`/`Persistence`/`Infrastructure` -> `Domain`.
`Domain` has no dependency on any other project.

## Domain model

- **Company** (aggregate root) — an employer with a name and contact email.
- **Job** (aggregate root) — a job posting owned by a Company. Lifecycle: `Draft` -> `Published` -> `Closed`.
  Owns its `JobApplication` child entities and is the only entry point for submitting, shortlisting,
  accepting and rejecting applications.
- **JobApplication** (child entity) — a candidate's application to a Job.

Domain rules are enforced on the aggregates and return a `Result`/`Result<T>` instead of throwing for
expected failures (e.g. publishing an already-published job). Each state transition raises a domain
event (`JobPublishedDomainEvent`, `JobApplicationSubmittedDomainEvent`, etc.) that Infrastructure
handlers react to.

## Running locally

```bash
cd backend
dotnet ef database update --project src/JobBoard.Persistence --startup-project src/JobBoard.Api
dotnet run --project src/JobBoard.Api
```

The API listens on the URL printed by `dotnet run` (see `src/JobBoard.Api/Properties/launchSettings.json`).
In Development, API docs are served via Scalar at `/scalar/v1`.

Data is stored in a local SQLite file (`jobboard.db`) using the `ConnectionStrings:JobBoard` setting in
`src/JobBoard.Api/appsettings.json`.

### Adding a migration

```bash
dotnet ef migrations add <Name> --project src/JobBoard.Persistence --startup-project src/JobBoard.Api
```

## Endpoints

| Method | Route                          | Description                     |
|--------|---------------------------------|----------------------------------|
| POST   | `/api/companies`                | Register a company               |
| POST   | `/api/jobs`                     | Create a draft job posting       |
| GET    | `/api/jobs`                     | List all jobs                    |
| GET    | `/api/jobs/{id}`                | Get a job by id                  |
| POST   | `/api/jobs/{id}/publish`        | Publish a draft job               |
| POST   | `/api/jobs/{id}/close`          | Close a published job             |
| POST   | `/api/jobs/{id}/applications`   | Apply to a published job          |
