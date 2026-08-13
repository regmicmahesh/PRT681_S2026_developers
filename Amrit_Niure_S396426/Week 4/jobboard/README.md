# JobBoard

A job board built with Clean Architecture and Domain-Driven Design: a .NET 10 Web API backend and a
React + TypeScript frontend.

```
jobboard/
  backend/    .NET solution (Domain, Application, Persistence, Infrastructure, Api) - see backend/README.md
  frontend/   React + Vite + TypeScript app - see frontend/README.md
```

## Domain

A **Company** posts **Job** openings. A Job moves through `Draft -> Published -> Closed`; once published,
candidates can submit a **JobApplication**, which a company can shortlist, accept or reject. This
lifecycle - and the rules around it (only draft jobs publish, only published jobs accept applications,
etc.) - lives entirely in the `Job` aggregate in `backend/src/JobBoard.Domain`, not scattered across
handlers or controllers.

## Architecture

Both sides follow the same idea: dependencies point inward, toward the domain model, never outward
toward frameworks or delivery mechanisms.

**Backend** (Clean Architecture):

```
Domain          <- Application  <- Api
                <- Persistence  <-'
                <- Infrastructure <-'
```

- `Domain` has no dependencies on anything else - entities, value objects, domain events, repository
  interfaces.
- `Application` orchestrates use cases as CQRS commands/queries (via MediatR) against the Domain.
- `Persistence` implements the repositories with EF Core (SQLite for local dev).
- `Infrastructure` reacts to domain events for cross-cutting concerns (currently: notification emails).
- `Api` is the composition root: controllers, dependency injection wiring, Scalar API docs.

**Frontend** (layered, mirroring the same inward-pointing dependency rule):

```
domain          <- application <- presentation
infrastructure  <-'            <-'
```

- `domain` - TypeScript types shared across the app (`Job`, `Company`).
- `infrastructure` - the HTTP client and typed API wrappers; the only code that knows the backend's URLs.
- `application` - TanStack Query hooks (`useJobs`, `useCreateJob`, ...) that call infrastructure and
  expose server state to components.
- `presentation` - pages and components that consume the application hooks.

## Running everything locally

```bash
# Terminal 1: backend
cd backend
dotnet ef database update --project src/JobBoard.Persistence --startup-project src/JobBoard.Api
dotnet run --project src/JobBoard.Api

# Terminal 2: frontend
cd frontend
npm install
npm run dev
```

The frontend defaults to `http://localhost:5289/api` for the backend (see `frontend/.env.example`); the
backend's CORS policy allows `http://localhost:5173`, Vite's default dev port.

See `backend/README.md` and `frontend/README.md` for details on each side.
