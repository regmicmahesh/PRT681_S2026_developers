# CareerTrack NT — Project Specification

## Status and evidence boundary

This is Maris Nguyen's individual proof-of-concept for the PRT585/PRT681 Developer role.

The application and documents are completed work only where the repository contains verifiable output. Group agreement, meetings, presentations, course completion, tool installation on another computer, and timesheet hours must be recorded by Maris after they occur.

## Objective

CareerTrack NT helps a job seeker record applications, see the current stage, manage follow-up dates, and keep concise notes. The portfolio demonstrates how requirements become validation rules, API behaviour, database records, tests, and an accessible interface.

Primary user: a job seeker managing multiple applications.

Success means the user can:

1. create an application with valid company, role, status and dates;
2. view and filter a paginated application list;
3. update an application's status and details;
4. delete an application only after a confirmation step in the interface; and
5. receive clear, safe validation and error messages.

## Scope

### Included

- Three-tier structure: presentation, application/API, and data layers.
- REST-style JSON API.
- SQLite persistence using Node's built-in `node:sqlite` module.
- Responsive browser interface using semantic HTML, CSS and JavaScript.
- Server-side and client-side validation.
- Unit and API integration tests using Node's built-in test runner.
- Security headers, request-size limits, parameterised database statements and generic server errors.
- Developer artefacts: architecture, data model, API contract, validation, source code, automated tests and technical notes.

### Excluded from this proof-of-concept

- Authentication, multiple users and authorisation.
- CV/document uploads.
- Email, calendar or job-board integrations.
- Deployment to a public environment.
- Claims that ASP.NET Core, Entity Framework, React or external courses were completed.

These exclusions prevent unnecessary collection of personal data and keep the assignment demonstrable with the installed local toolchain.

## Technology and architecture

- Runtime: Node.js 25 or later.
- Database: SQLite through the built-in `node:sqlite` API.
- API server: Node built-in `http` module; no third-party runtime dependencies.
- Front end: HTML, CSS and browser JavaScript served by the API host.
- Tests: `node:test` and `node:assert`.

Dependency direction:

```text
Browser UI -> HTTP routes -> Application service -> Repository -> SQLite
```

The HTTP layer does not contain SQL. The repository does not know about HTTP. Business validation is centralised in the application layer.

## Commands

Run from `CareerTrackNT`:

```powershell
npm.cmd test
npm.cmd run check
npm.cmd start
```

Open `http://localhost:3000` after starting the server. In PowerShell, run `$env:PORT = "3001"` before `npm.cmd start` when port 3000 is unavailable.

## Project structure

```text
CareerTrackNT/
  src/domain/       validation and business rules
  src/data/         SQLite schema and parameterised repository
  src/http/         HTTP parsing, routing and safe responses
  public/           accessible browser interface
  test/             unit and API integration tests
  data/             runtime database, ignored by Git
  docs/             API and technical notes
```

## Code style

- ES modules and explicit named exports.
- Descriptive names such as `applicationRepository`, never ambiguous names such as `data`.
- Small functions with one responsibility.
- API errors use a stable `{ error: { code, message, details? } }` shape.
- Dates use ISO `YYYY-MM-DD` strings at the API boundary.

Example:

```js
export function normaliseApplication(input) {
  return {
    company: input.company.trim(),
    role: input.role.trim(),
    status: input.status,
  };
}
```

## Validation rules

- Company: required, trimmed, 2–100 characters.
- Role: required, trimmed, 2–120 characters.
- Status: one of `Wishlist`, `Applied`, `Interview`, `Offer`, `Rejected`, `Withdrawn`.
- Application date: required for all stages except `Wishlist`; valid ISO date; cannot be in the future.
- Follow-up date: optional valid ISO date; cannot be before the application date.
- Job URL: optional; must use `https://` and be at most 500 characters.
- Notes: optional, trimmed, at most 1,000 characters.

## API contract

- `GET /api/applications?page=1&pageSize=20&status=Applied&search=engineer`
- `GET /api/applications/:id`
- `POST /api/applications`
- `PUT /api/applications/:id`
- `DELETE /api/applications/:id`
- `GET /api/summary`
- `GET /api/health`

List requests are bounded to a maximum page size of 100. Unknown fields are ignored; returned objects contain only the documented fields.

## Testing strategy

- Unit tests cover validation, normalisation and date rules.
- Integration tests start the real HTTP server against an in-memory SQLite database.
- Tests assert observable outcomes rather than internal function calls.
- Each test owns its database and closes it after use.

## Threat model

Trust boundaries are browser-to-API input and API-to-database storage. Assets are application history and notes. Relevant misuse cases include oversized requests, script markup in notes, SQL injection strings, invalid identifiers and unbounded list queries.

Controls:

- validate and length-limit all input at the API boundary;
- use parameterised SQLite statements;
- render user content with `textContent`, never `innerHTML`;
- cap JSON bodies at 64 KB and list page size at 100;
- return generic 500 responses without stack traces;
- set CSP, frame, MIME-sniffing, referrer and permissions headers;
- store no credentials, resumes or secrets.

Authentication is excluded, so this is a local single-user learning application and must not be exposed publicly.

## Boundaries

Always:

- run tests and syntax checks after behaviour changes;
- validate input on the server;
- use parameterised statements;
- keep generated databases and secrets out of Git;
- preserve truthful evidence status in documentation.

Ask first:

- introduce authentication or personally identifiable information;
- add external services or dependencies;
- change the database schema after shared integration begins;
- publish or deploy the application.

Never:

- commit credentials, tokens, real application notes or resumes;
- claim meetings, hours, training or group approval that did not occur;
- render untrusted strings as HTML;
- expose this unauthenticated prototype publicly.

## Definition of done

- All specified endpoints have tests for success and important error paths.
- `npm.cmd test` passes without skipped tests.
- `npm.cmd run check` passes.
- A user can create, filter, edit and delete an application through the interface.
- The UI includes loading, error and empty states and supports keyboard use.
- API and run instructions are documented.
- Developer acceptance criteria trace to implementation and automated tests.
- No secrets or generated database files are tracked.

## Open items requiring Maris or the group

- Confirm whether the group product is CareerTrack NT; otherwise label this as an individual POC.
- Install the .NET SDK and complete an ASP.NET Core/Entity Framework port if the lecturer requires that exact stack.
- Complete the selected learning materials and record only actual completion.
- Record real meetings, presentations, Teams posts and timesheet hours.
