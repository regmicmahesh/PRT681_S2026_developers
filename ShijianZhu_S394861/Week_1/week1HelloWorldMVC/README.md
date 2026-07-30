# week1HelloWorldMVC

ASP.NET Core MVC Week 1 practice project by Shijian Zhu (`S394861`).

## Features

- HelloWorld controller with Index and Welcome views.
- Movie model with title, release date, genre, price, and Rating.
- SQL Server Express LocalDB configuration through EF Core.
- Asynchronous Movies Create, List, Details, Edit, and Delete actions.
- Partial title/genre search with an optional Rating filter.
- Data Annotations validation with client-side validation messages.
- Initial EF Core migration.
- Automatic migration and sample data for a new development database.

## Prerequisites

- .NET 10 SDK
- Windows with SQL Server Express LocalDB

If LocalDB is missing, install it through Visual Studio Installer as an individual
component or from SQL Server Express media. Official guidance:
https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb

Check LocalDB:

```powershell
sqllocaldb info
```

## Restore, create the database, and run

```powershell
dotnet restore
dotnet tool restore
dotnet ef database update
dotnet run
```

In Development, the application also applies pending migrations automatically
and inserts three sample Movies only when the database is empty.

Open the URL shown in the terminal, then use:

- `/HelloWorld`
- `/HelloWorld/Welcome?name=Shijian&numTimes=3`
- `/Movies`

## Validation examples

- A title shorter than three characters is rejected.
- A future release date is rejected.
- Price must be from `0.01` to `1000.00`.
- Rating must be selected from the supported Australian/international examples.

## Automated tests

From the parent `Week_1` directory:

```powershell
dotnet test ShijianZhu.Week1.slnx --configuration Release
```

The test project covers Movie validation plus title/genre search and Rating
filtering with an isolated EF Core InMemory database.

## References

- https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc
- https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql
- https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
