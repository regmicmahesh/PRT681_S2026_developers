# CareerTrack NT

A local full-stack proof-of-concept for recording job applications, stages and follow-up dates. It uses Node.js, a REST-style HTTP API, built-in SQLite, semantic HTML/CSS/JavaScript and automated tests.

## Prerequisites

- Node.js 25 or later (the application uses built-in `node:sqlite`).
- No package installation or external service is required.

## Run

```powershell
cd "C:\Users\zellk\Desktop\Code\CDU\PRT681_S2026_developers\Maris_S379862\CareerTrackNT"
npm.cmd test
npm.cmd run check
npm.cmd start
```

Open <http://127.0.0.1:3000>.

The runtime database is created at `data/careertrack.db` and ignored by Git. Use fictional data only: the POC has no authentication and is not approved for public deployment.

## Features

- Create, retrieve, update and delete applications.
- Search company/role and filter by status.
- Bounded pagination and pipeline summary.
- Shared server validation and database constraints.
- Responsive, keyboard-accessible interface with loading, empty, error and success states.
- Parameterised SQL and security headers.
- Unit and real HTTP/SQLite integration tests.

## Project layout

```text
src/domain/  rules, errors and application service
src/data/    schema and repository
src/http/    routes, JSON parsing and static hosting
public/      browser interface
test/        unit and integration tests
docs/        API and manual verification notes
```

## Verification performed

- `npm.cmd test`: 22 tests passed at the final automated-verification checkpoint.
- `npm.cmd run check`: 12 JavaScript files passed syntax checking at the checkpoint.

Re-run both commands after future changes; the numbers may increase as tests/files are added.

## Known boundaries

- No sign-in or per-user authorisation.
- No external reminders, uploads or job-board integrations.
- No React build in this POC; React remains a separate learning task.
- No ASP.NET Core/EF Core build because the .NET SDK was absent in the verified environment.
