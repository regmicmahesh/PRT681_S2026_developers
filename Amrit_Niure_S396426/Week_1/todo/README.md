# Solution-Level Configuration Files

This solution follows the standard .NET convention of putting shared, cross-project
configuration in a small set of well-known files at the **solution root** (the same
folder as `todo.slnx`, one level above every project folder). MSBuild and the .NET
CLI auto-discover these files by walking *up* the directory tree from each `.csproj`
— nothing needs to reference them explicitly.

**Where to put them:** solution root, not inside any individual project folder, and
not further up outside the solution (e.g. not in the repo root if the repo contains
multiple unrelated solutions). Auto-discovery works by searching upward from each
project until it finds the file, so placing them any higher would risk a different
`Directory.Build.props`/`Directory.Packages.props` bleeding into unrelated solutions
that happen to live in a parent folder; placing them lower (inside a project folder)
means other projects in the solution won't pick them up at all.

## Files in this solution

### `Directory.Build.props`
General MSBuild **properties** shared by every project in the solution — things like
`TargetFramework`, `Nullable`, `ImplicitUsings`, language version, or analyzer
settings. Imported very early in the build, before the SDK sets its own defaults, so
anything set here behaves like a default that an individual `.csproj` can still
override locally if it needs to.

In this solution it hoists `TargetFramework`, `ImplicitUsings`, and `Nullable` out of
Domain, Application, Infrastructure, Presentation, and WebAPI so they're declared
once instead of five times.

### `Directory.Build.targets`
The sibling of `Directory.Build.props`, using the same auto-discovery, but imported
at the very **end** of the build instead of the start — after the SDK has already
populated its own items and properties. Use it when you need to react to or extend
something the SDK sets up (e.g. appending to an SDK-populated item group, adding a
post-build step). Not present in this solution yet — most small-to-mid solutions
never need one.

### `Directory.Packages.props`
Enables **Central Package Management (CPM)** for NuGet. Sets
`ManagePackageVersionsCentrally=true` and lists every package's version once via
`<PackageVersion Include="..." Version="..." />`. Individual `.csproj` files then
reference packages **without** a version:
`<PackageReference Include="MediatR" />`. This guarantees every project in the
solution uses the exact same version of a given package and makes version bumps a
one-line change instead of a find-and-replace across every `.csproj`.

> Note the file name is `Directory.Packages.props`, distinct from
> `Directory.Build.props` — MSBuild only recognizes each by its exact name. A typo
> here (e.g. `DirectoryPackages.props`) fails silently: the file is just never
> imported, `ManagePackageVersionsCentrally` never gets set, and every
> version-less `PackageReference` in the solution fails to restore with `NU1015`.

### `global.json` *(not yet added)*
Pins the exact .NET SDK version the solution builds with, so every developer
machine and CI agent uses the same SDK regardless of what else is installed
globally. Prevents "works on my machine" issues caused by SDK version drift.

### `.editorconfig` *(not yet added)*
Repo-wide code style and analyzer severity rules enforced by the compiler/IDE —
indentation, naming conventions, `dotnet_diagnostic.*` warning levels. Keeps
formatting and analyzer behavior consistent across every editor/IDE a contributor
might use.

### `nuget.config` *(not yet added)*
Repo-level NuGet settings — which package feeds to use, credentials for private
feeds, whether to fall back to nuget.org. Only needed once the solution consumes
packages from somewhere other than the public NuGet feed.

### `packages.lock.json` *(not yet added)*
Opt-in via `RestorePackagesWithLockFile=true`. Locks the exact resolved version of
every **transitive** dependency (not just the ones you reference directly), so a
`dotnet restore` produces byte-identical dependency graphs across machines and over
time — the NuGet equivalent of npm's `package-lock.json`.

## Recommended baseline for a new clean-architecture solution

```
Directory.Build.props     ← shared TargetFramework / Nullable / LangVersion / analyzers
Directory.Packages.props  ← central NuGet versions (CPM)
.editorconfig             ← style + analyzer rules
global.json                ← SDK version pin
```

All four live at the solution root, next to the `.slnx`/`.sln` file. This is the
combination most .NET teams treat as baseline hygiene for any multi-project
solution — it removes duplicated settings from every `.csproj` and makes the whole
solution build the same way on every machine.

# The Todo App: Clean Architecture + DDD Reference

This solution implements a small but complete Todo API to demonstrate how Clean
Architecture's layers and Domain-Driven Design's building blocks fit together in
practice. It is meant to be read top-to-bottom as a worked example, not copied
blindly — the same shapes will look different (and often simpler) for a domain
that doesn't need them.

## The four layers and the dependency rule

```
Domain            ← no project references. Depends on nothing.
   ^
Application       ← references Domain only.
   ^
Infrastructure    ← references Domain only (implements Domain's interfaces).
   ^
Presentation      ← references Domain + Application.
   ^
WebAPI            ← references Application + Infrastructure + Presentation
                     (the only project that knows all four exist).
```

Dependencies only point **inward**, toward Domain. Domain never references
Application, Infrastructure, or Presentation — it has zero project references at
all. This is "the dependency rule": the business logic doesn't know or care that
EF Core, ASP.NET Core, or MediatR exist. You could delete Infrastructure and
Presentation entirely and Domain would still compile.

## Domain layer (`Domain/`)

The business logic and rules, with no framework dependencies (MediatR is the one
exception — see the note in `Domain/Common/IDomainEvent.cs`'s usage below — it's a
pragmatic, widely-used trade-off, not a violation of the dependency rule in
spirit).

| Folder | DDD concept | What it is here |
|---|---|---|
| `Common/BaseEntity.cs` | **Entity** | Base class for anything with an identity (`Id`) that persists across changes. Equality is by `Id`, not by field values. |
| `Common/AggregateRoot.cs` | **Aggregate Root** | Marks the single entry point of a consistency boundary. Repositories only ever load/save aggregate roots — never the entities/value objects nested inside them directly. |
| `Common/ValueObject.cs` | **Value Object** | Base class for things with no identity of their own — two instances are equal when their components match, not when their `Id` matches (they don't have one). |
| `ValueObjects/TodoTitle.cs` | **Value Object** | A concrete example: wraps a raw `string` so "must be 1–200 characters" can never be bypassed — any `TodoTitle` in memory is valid by construction. |
| `Enums/Priority.cs` | **Enum** | A closed set of domain values (`Low`/`Medium`/`High`). |
| `Entities/TodoItem.cs` | **Aggregate Root / Entity** | The one aggregate in this domain. All mutation happens through named methods (`Complete()`, `Reopen()`, `UpdateDetails(...)`) that enforce invariants — there are no public setters, so it's impossible to construct a `TodoItem` in an invalid state or mutate it into one. |
| `Events/TodoItemCreatedEvent.cs`, `TodoItemCompletedEvent.cs` | **Domain Event** | Facts about something that already happened in the domain. Raised inside `TodoItem`'s methods, dispatched later by Infrastructure (see `UnitOfWork` below) — so a handler can react (e.g. logging, sending a notification) without the aggregate knowing who's listening. |
| `Exceptions/DomainException.cs`, `EntityNotFoundException.cs` | **Domain Exception** | Thrown when an operation would violate a business rule the domain owns (e.g. completing an already-completed item). |
| `Repositories/ITodoItemRepository.cs`, `IUnitOfWork.cs` | **Repository interface** | Domain declares *what* persistence operations it needs; it has no idea *how* they're implemented. Infrastructure provides the implementation. This is what lets Domain have zero project references. |

## Application layer (`Application/`)

Orchestrates use cases using **CQRS** (Command Query Responsibility Segregation)
via [MediatR](https://github.com/jbogard/MediatR): every use case is its own
Command (writes) or Query (reads) plus a matching Handler, instead of one fat
`TodoService` class with a method per operation.

```
TodoItems/
  Commands/
    CreateTodoItem/   → CreateTodoItemCommand, ...Validator, ...Handler
    UpdateTodoItem/
    CompleteTodoItem/
    ReopenTodoItem/
    DeleteTodoItem/
  Queries/
    GetTodoItems/     → GetTodoItemsQuery, ...Handler
    GetTodoItemById/
  Dtos/TodoItemDto.cs  → shape returned to the outside world (never leaks Domain entities)
  EventHandlers/        → INotificationHandler<T> for each Domain event, e.g. logging
Common/
  Behaviors/ValidationBehavior.cs → runs FluentValidation before any handler executes
  Exceptions/ValidationException.cs
```

Handlers depend only on Domain's repository interfaces (`ITodoItemRepository`,
`IUnitOfWork`) — never on EF Core or any concrete Infrastructure type. Validators
(FluentValidation) run automatically for every request through
`ValidationBehavior`, a MediatR *pipeline behavior* — a cross-cutting concern
wired in once (`Application/DependencyInjection.cs`) instead of called manually
at the top of every handler.

## Infrastructure layer (`Infrastructure/`)

Implements the interfaces Domain declared, using EF Core + SQLite:

- `Persistence/TodoDbContext.cs` — the EF Core `DbContext`.
- `Persistence/Configurations/TodoItemConfiguration.cs` — maps the `TodoTitle`
  value object to a plain `string` column via `HasConversion`, since EF Core only
  understands primitive columns, not domain value objects.
- `Persistence/Repositories/TodoItemRepository.cs` — implements
  `ITodoItemRepository` using `DbSet<TodoItem>`.
- `Persistence/UnitOfWork.cs` — implements `IUnitOfWork`. This is where domain
  events actually get dispatched: after `SaveChangesAsync` commits, it walks the
  tracked aggregates that raised events and publishes each one through MediatR's
  `IPublisher`, which is what invokes the `INotificationHandler`s in Application.
- `Persistence/Migrations/` — EF Core migrations, generated with:
  ```
  dotnet ef migrations add <Name> --project Infrastructure --startup-project WebAPI --output-dir Persistence/Migrations
  ```

## Presentation layer (`Presentation/`)

The HTTP surface, kept separate from `WebAPI` so the host project (`WebAPI`) only
has to wire things together, not define them:

- `Endpoints/TodoItemEndpoints.cs` — ASP.NET Core minimal API endpoints
  (`MapGet`/`MapPost`/...), each one just sending a Command/Query through
  `ISender` and translating the result to an HTTP response.
- `ExceptionHandling/GlobalExceptionHandler.cs` — an `IExceptionHandler` that maps
  Domain/Application exceptions to HTTP status codes in one place
  (`EntityNotFoundException` → 404, `DomainException`/`ValidationException` → 400,
  anything else → 500), so handlers never think about status codes at all.

## WebAPI (`WebAPI/`)

The composition root. `Program.cs` calls `AddApplication()`, `AddInfrastructure()`,
`AddPresentation()` to register everything, then `app.MapTodoItemEndpoints()` and
`app.UseExceptionHandler()` to activate it. This is the only project that
references all the others — every other layer only knows about the layers inside
it.

## Trying it out

```
dotnet run --project WebAPI
```

Then, against `http://localhost:5145/api/todos`:

| Verb | Route | Body | Notes |
|---|---|---|---|
| GET | `/api/todos` | – | optional `?isCompleted=true` filter |
| GET | `/api/todos/{id}` | – | 404 if missing |
| POST | `/api/todos` | `{ "title", "description", "priority", "dueDate" }` | 400 if title empty |
| PUT | `/api/todos/{id}` | same shape | 400 if the item is already completed |
| POST | `/api/todos/{id}/complete` | – | 400 if already completed |
| POST | `/api/todos/{id}/reopen` | – | 400 if not completed |
| DELETE | `/api/todos/{id}` | – | 404 if missing |

`priority` is `0` = Low, `1` = Medium, `2` = High.
