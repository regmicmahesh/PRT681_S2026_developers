# Week 3 — POC, Validation, Migrations and UI Libraries

## CareerTrack NT POC

Theme: a practical job-application workspace for Darwin/NT job searching.

Implemented vertical slices:

1. Create a validated application (data + application logic + API + UI).
2. Search/filter and paginate applications.
3. Retrieve and update an application.
4. Confirm and delete an application.
5. Aggregate and display pipeline counts.

The interface uses a restrained dark-green palette, semantic controls, visible focus, responsive layout and meaningful states rather than a generic card/gradient template.

## Validation approach

### Current implementation

- Browser: required/type/length rules provide early feedback.
- Server: one validation module enforces all business rules because browser rules can be bypassed.
- Database: `NOT NULL`, length and status `CHECK` constraints add defence in depth.
- Tests: valid, missing, invalid status, impossible/future date, date order and URL cases.

### FluentValidation mapping for a future .NET port

FluentValidation expresses rules in a dedicated validator class. A conceptual mapping is:

```csharp
RuleFor(x => x.Company)
    .NotEmpty()
    .Length(2, 100);

RuleFor(x => x.Status)
    .Must(status => AllowedStatuses.Contains(status));

RuleFor(x => x.FollowUpDate)
    .GreaterThanOrEqualTo(x => x.ApplicationDate)
    .When(x => x.FollowUpDate.HasValue && x.ApplicationDate.HasValue);
```

This snippet is uncompiled research because the .NET SDK is unavailable. A real port must add tests for the validator and confirm package/version documentation.

## Migration approaches

### EF Core migrations

Generated from model changes and applied through the .NET toolchain. Benefits include integration with the data model; risks include accepting generated changes without reviewing SQL or rollback/production strategy.

### FluentMigrator

Uses explicit C# migration classes with `Up` and `Down` methods. It can suit teams that want migrations independent of an ORM. A migration must be ordered, reviewed, tested against representative data and backed by deployment/rollback planning.

### Current POC

The SQLite schema is created idempotently for learning. This is sufficient for a local POC but not a production migration history. Schema evolution is a documented future task.

## UI library evaluation

| Option | Strengths | Costs/risks | Decision |
|---|---|---|---|
| Native HTML/CSS | Small, fast, accessible primitives, no licence/dependency | More custom work for advanced grids | Selected for current POC |
| Kendo UI | Mature grids, filters and enterprise widgets | Licence, bundle size, theming and accessibility verification | Trial only if group has access and advanced grid need |
| DevExpress | Broad component suite and data controls | Licence and integration complexity | Compare only after concrete requirements |
| Bootstrap | Familiar responsive primitives | Generic appearance and unused CSS risk | Not required for current layout |

The correct choice depends on product needs, accessibility, support, performance and licence—not the number of available components.

## Alternative frontend learning

The lecturer suggests trying React, Vue or Angular alternatives. Maris already has Angular experience and should prioritise a small React API-consuming exercise next. It should remain separate from the working POC until its build/test setup is verified.

## Quality evidence

- [x] Server and client validation designed at correct boundaries.
- [x] Parameterised SQL and bounded list endpoint.
- [x] Loading, empty, error and success UI states.
- [x] Responsive and keyboard-focused design implemented.
- [x] Automated unit and API integration tests.
- [ ] React alternative implemented.
- [ ] FluentValidation and migration examples compiled under .NET.
- [ ] Third-party UI library trial completed/presented.
- [ ] Manual browser/accessibility checklist executed.
