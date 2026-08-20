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

- **Company** (aggregate root) — an employer with a name, contact email, and an `OwnerId` (the Auth
  service user id of whoever created it). `OwnerId` is the ownership boundary the authorization
  layer scopes job management to - see [Authorization](#authorization) below.
- **Job** (aggregate root) — a job posting owned by a Company. Lifecycle: `Draft` -> `Published` -> `Closed`.
  Owns its `JobApplication` child entities and is the only entry point for submitting, shortlisting,
  accepting and rejecting applications.
- **JobApplication** (child entity) — a candidate's application to a Job.

Domain rules are enforced on the aggregates and return a `Result`/`Result<T>` instead of throwing for
expected failures (e.g. publishing an already-published job). Each state transition raises a domain
event (`JobPublishedDomainEvent`, `JobApplicationSubmittedDomainEvent`, etc.) that Infrastructure
handlers react to.

## Authorization

This service issues no tokens of its own - it's a resource server that trusts JWTs signed by the
`Auth` service (Week 4/Auth). It validates them (`Jwt:Issuer`/`Jwt:Audience`/`Jwt:SecretKey` in
config must match the Auth service exactly) and reads the `permission` claims it carries
(`job:create`, `job:update`, `job:apply`, etc.), using the same permission-policy convention as the
Auth service. See `src/JobBoard.Api/Authorization/` and the Auth service's `Authorization/README.md`.

Listing/browsing (`GET /api/companies`, `GET /api/jobs`, `GET /api/jobs/{id}`) is anonymous - a job
board is meant to be publicly browsable. Every mutating endpoint requires a permission, and
`POST /api/jobs`, `.../publish`, `.../close` additionally require the caller to own the job's
company (`CompanyOwnerOrPermissionRequirement`) - `job:create`/`job:update`/`job:delete` alone are
granted to an entire Employer/Recruiter role, not to one company, so the permission check alone
can't stop one Employer from editing another Employer's postings.

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

Requires `Jwt:SecretKey` in user secrets, set to the *same* value as the Auth service's
(`dotnet user-secrets list --project ../Auth/Auth/Auth`), since this service validates tokens the
Auth service signs. `Jwt:Issuer`/`Jwt:Audience` already match in `appsettings.json`.

### Adding a migration

```bash
dotnet ef migrations add <Name> --project src/JobBoard.Persistence --startup-project src/JobBoard.Api
```

## Endpoints

| Method | Route                          | Description                     | Auth |
|--------|---------------------------------|----------------------------------|------|
| GET    | `/api/companies`                | List all companies               | Anonymous |
| POST   | `/api/companies`                | Register a company (caller becomes its owner) | `job:create` |
| POST   | `/api/jobs`                     | Create a draft job posting       | `job:create` + owns `CompanyId` |
| GET    | `/api/jobs`                     | List all jobs                    | Anonymous |
| GET    | `/api/jobs/{id}`                | Get a job by id                  | Anonymous |
| POST   | `/api/jobs/{id}/publish`        | Publish a draft job               | `job:update` + owns the job's company |
| POST   | `/api/jobs/{id}/close`          | Close a published job             | `job:update` + owns the job's company |
| POST   | `/api/jobs/{id}/applications`   | Apply to a published job          | `job:apply` |

`job:manage-any` (Admin only) bypasses the "owns the company" checks above.
