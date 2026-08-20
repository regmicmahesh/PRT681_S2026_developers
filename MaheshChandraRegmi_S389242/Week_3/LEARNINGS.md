# Week 3 — turning the expense tracker into an API

Week 1 I had the expense tracker as a console app (then OOP, then a SQL version). Week 1 WebApi was just todos. This week I wanted the actual expense tracker on HTTP so I can call it from swagger instead of typing into a menu loop.

## What I built

`ExpenseTrackerApi` — same features as the CLI:

- add an expense
- list all expenses
- get one by id
- update
- delete
- total + average (`GET /api/expenses/summary`)
- filter over a threshold (`GET /api/expenses?minAmount=50`)
- categories from the DbApp work (Food / Transport / Rent / Other)

## Things I actually had to sit with

**DTOs vs entities.** In Week 1 I returned the `Todo` entity straight from the controller. That works until you include `Category` and suddenly you get a json cycle (`Category.Expenses` -> expense -> category...). So this week I map to `ExpenseDto` before sending anything out.

**Validation.** `[Required]`, `[Range]`, `[MinLength]` on the create/update records. `[ApiController]` turns those into 400 automatically. Way nicer than `TryParse` in a while loop.

**Interface + DI.** Week 1 I registered `SqliteRepository` as itself. This time I did `IExpenseRepository` -> `ExpenseRepository` so the tests (and later a different storage) can swap.

**Tests.** First time writing xUnit against EF. In-memory sqlite (`Data Source=:memory:`) was the trick — you have to keep the connection open or the db vanishes. I tested the same math the CLI used: total, average, filter, delete.

## How to run

```bash
cd Week_3
dotnet test
dotnet run --project ExpenseTrackerApi
```

Swagger: http://localhost:5031/swagger
