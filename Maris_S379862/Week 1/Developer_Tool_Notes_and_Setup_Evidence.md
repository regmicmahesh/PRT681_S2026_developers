# Week 1 — Developer Tool Notes and Setup Evidence

## Environment checks performed in this workspace

| Tool | Observed result | Meaning/action |
|---|---|---|
| Node.js | v25.8.1 | Available; includes built-in SQLite module used by POC |
| npm | 11.11.0 | Available for project scripts |
| Git | Available through repository commands | Repositories can be inspected; commits remain Maris's responsibility |
| .NET SDK | Command unavailable; standard install locations absent | Install and verify before claiming ASP.NET/EF Core completion |
| SQLite | `node:sqlite` in-memory query succeeded | Available through current Node runtime |
| React/Vite | Not installed in the POC | React learning/implementation remains pending |
| Visual Studio/SSMS/Postman | Not verifiable from this execution environment | Maris must record installation/version evidence on the actual study machine |

## Concise technology notes

### Git

Git records changes as commits and enables branching, review and rollback. Small focused commits make collaboration safer than one large mixed change. A shared repository requires pulling/reviewing before pushing and changing only the assigned folder. Runtime databases, secrets and personal data belong in ignore rules, not Git history.

### HTTP and REST APIs

HTTP carries requests and responses using methods, URLs, headers, status codes and bodies. A REST-style API models resources consistently; it is not simply “JSON over the internet.” CareerTrack NT uses GET, POST, PUT and DELETE with 201, 204, 400, 404, 415 and 422 outcomes. Stable error shapes help the UI handle failures predictably.

### JavaScript and Node.js

JavaScript runs in the browser; Node.js runs JavaScript on the server. Node is well suited to I/O-heavy APIs and provides a standard test runner. The POC uses only built-in modules, reducing dependency and installation risk. Asynchronous request handling is kept separate from synchronous, small SQLite operations.

### React

React builds user interfaces from components and state. It is useful for complex forms, dashboards and reusable interaction patterns. A React Hello World should demonstrate component rendering, props/state and an API call—not only text on screen. React remains planned because it is not installed or built in this POC.

### C# and ASP.NET Core

C# is a strongly typed language in the .NET ecosystem. ASP.NET Core supports web APIs, middleware, dependency injection, configuration and hosted services. It matches the lecturer's intended enterprise pathway. The current machine check found no .NET SDK, so installation and a runnable port are honest backlog items.

### ORM and Entity Framework Core

An ORM maps objects and queries to relational storage. EF Core provides change tracking, relationships and migrations for .NET applications. An ORM reduces repetitive mapping but does not replace database design, indexes or query analysis. The current repository abstraction creates a clean boundary for a later EF Core implementation.

### SQLite and SQL Server

SQLite is embedded and file-based, making it useful for prototypes and tests. SQL Server provides broader administration, concurrency, security and enterprise capabilities. Both use relational modelling, constraints and indexes, although dialect/features differ. CareerTrack NT uses SQLite locally and keeps SQL Server study separate.

### Postman and automated API testing

Postman is useful for exploratory requests and demonstrations. Automated integration tests are repeatable and protect behaviour on every change. A good API learning workflow uses both: explore manually, then encode important success/error cases as tests. The POC currently has automated API coverage; Postman evidence is pending.

### Validation

Validation protects data quality and security at the system boundary. Browser validation improves usability but can be bypassed, so the server must enforce every rule. CareerTrack NT validates lengths, stage allowlists, real ISO dates, date order and HTTPS URLs. Database constraints add defence in depth.

### Docker

Docker packages an application and runtime settings as a container image. It improves repeatability but does not automatically solve persistent storage, secrets, monitoring or security. A useful exercise would add a minimal image only after the application and runtime requirements are stable. No new container evidence is claimed here.

## Week 1 POC evidence

- [x] Project specification and task plan written.
- [x] Node/SQLite runtime proved locally.
- [x] Validation tests failed before implementation and passed after it.
- [x] Create, retrieve and bounded list/filter behaviour implemented.
- [x] Input is parameterised at the database boundary.
- [x] Test suite and syntax-check scripts run successfully.
- [ ] .NET, Visual Studio, SQL Server Express and SSMS verified by Maris.
- [ ] Postman screenshots/collection created by Maris.
- [ ] Group checkout/run/commit and screen-share evidence recorded.
