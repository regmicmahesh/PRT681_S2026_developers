# Setup and Verification Evidence

## Environment

- Operating system: Windows
- .NET SDK used: 10.0.302
- Target framework: `net10.0`
- Data access: Entity Framework Core 10.0.4
- Database provider: SQL Server Express LocalDB
- Version control: Git and GitHub

## Commands used

```powershell
dotnet restore
dotnet build --no-restore
dotnet tool restore
dotnet ef database update
dotnet run
```

## Verification checklist

- [x] MVC project created.
- [x] HelloWorld controller and views added.
- [x] Movie model and database context added.
- [x] SQL Server/LocalDB connection configured.
- [x] Initial EF Core migration generated.
- [x] Movies CRUD views added.
- [x] Search and Rating field added.
- [x] Model validation added.
- [x] Debug and Release builds completed with 0 warnings and 0 errors.
- [ ] Local database creation (blocked on this computer because the LocalDB Runtime
  is not currently installed).

The LocalDB database can be created on a Windows computer with SQL Server Express
LocalDB installed by running `dotnet tool restore` followed by
`dotnet ef database update`.

Official LocalDB installation guidance:
https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb
