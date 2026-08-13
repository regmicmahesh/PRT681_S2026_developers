# JobBoard frontend

React + TypeScript app built with Vite, TanStack Query for server state, and React Router for navigation.

## Layers

```
src/
  domain/          TypeScript types shared across the app (Job, Company).
  infrastructure/   HTTP client + typed API wrappers - the only code that knows the backend's URLs.
  application/       TanStack Query hooks (useJobs, useCreateJob, ...) wrapping infrastructure.
  presentation/       Pages and components that consume the application hooks.
```

Dependencies point inward: `presentation` -> `application` -> `infrastructure` -> `domain`. `domain` has
no dependency on anything else.

## Running locally

```bash
npm install
npm run dev
```

Copy `.env.example` to `.env` if you need to point at a backend running somewhere other than
`http://localhost:5289/api`.

## Pages

| Route             | Page              | Description                                  |
|--------------------|--------------------|-----------------------------------------------|
| `/`                | JobListPage        | Lists all jobs                                |
| `/jobs/new`        | NewJobPage         | Post a new job (requires a company id)        |
| `/jobs/:id`        | JobDetailPage       | Job details, publish/close actions, apply form|
| `/companies/new`   | NewCompanyPage      | Register a company                            |
