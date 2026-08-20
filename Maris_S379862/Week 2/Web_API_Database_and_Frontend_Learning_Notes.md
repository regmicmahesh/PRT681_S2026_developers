# Week 2 — Web API, Database and Frontend Learning Notes

## .NET Web API with Entity Framework — planned pathway

The intended stack uses ASP.NET Core for routing/middleware and Entity Framework Core for relational persistence. A typical learning sequence is:

1. Install a supported .NET SDK and verify `dotnet --info`.
2. Create a web API project and test the generated health/sample endpoint.
3. Define an `Application` entity and request/response DTOs.
4. Configure a `DbContext` and a SQL Server connection through environment/user secrets.
5. Create and inspect the initial migration.
6. Implement validated create/list/update/delete endpoints.
7. Add integration tests against an isolated test database.
8. Compare all responses with the existing API contract before switching implementations.

This is research/planning only because the verified environment had no .NET SDK.

## React Hello World and API consumption — planned exercise

A meaningful React exercise should:

1. Create a component that renders a title and state value.
2. Update state from a button/form.
3. Fetch `GET /api/applications` in a lifecycle/effect appropriate to the selected React version.
4. Render loading, empty, success and error states.
5. Submit a validated record and refresh the list.
6. Test component behaviour through accessible roles/labels.

The current interface is executable standards-based JavaScript. It does not count as a completed React exercise.

## Three implemented API operations (plus additional operations)

- `POST /api/applications`: validate and create.
- `GET /api/applications`: bounded list/search/filter.
- `PUT /api/applications/:id`: validate and update.
- Additional: retrieve by ID, delete, summary and health.

See `CareerTrackNT/docs/API.md` for request/response examples.

## Database/ORM comparison

| Concern | Current POC | Intended .NET pathway |
|---|---|---|
| Database | SQLite | SQL Server |
| Mapping | Explicit repository mapping | EF Core entities/DTO projection |
| Schema change | SQL schema setup | Versioned EF Core migrations |
| Query safety | Bound parameters | LINQ/EF parameter binding |
| Tests | In-memory SQLite | Isolated test database/test container |

## Third-party UI library decision

Kendo UI and DevExpress provide rich data grids, filtering and controls but add licensing, bundle and learning considerations. The current POC uses native controls and a small responsive table, so it does not claim third-party library use. A future evaluation should verify licence terms, accessibility, bundle size and whether the product actually needs advanced grid behaviour.

## Evidence checklist

- [x] More than three API operations implemented and automatically tested.
- [x] Front end consumes the shared API.
- [x] Database-backed create/list/update/delete behaviour exists.
- [ ] .NET/EF Core tutorial watched and notes linked to completion evidence.
- [ ] React Hello World built and screenshot/test retained.
- [ ] Postman collection exported.
- [ ] Third-party component trial performed and presented.
