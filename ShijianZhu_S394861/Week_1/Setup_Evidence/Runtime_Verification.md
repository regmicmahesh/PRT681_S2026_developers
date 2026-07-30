# Runtime Verification

## Verification date

26 July 2026

## Environment result

- SQL Server Express LocalDB is installed.
- The `MSSQLLocalDB` instance is available.
- The ASP.NET Core application starts on `http://localhost:5192`.
- The LocalDB-backed Movies page returns data successfully.

## Read-only route checks

| Route | Result | Evidence observed |
|---|---|---|
| `/` | HTTP 200 | Home page rendered |
| `/HelloWorld` | HTTP 200 | HelloWorld MVC view rendered |
| `/Movies` | HTTP 200 | Movie list rendered from LocalDB |
| `/Movies?searchString=Diamond` | HTTP 200 | The `Diamond` record was returned |
| `/Movies?searchString=NoSuchMovie` | HTTP 200 | The no-results message was displayed |
| `/Movies/Create` | HTTP 200 | Create form contained client-validation attributes |
| `/Movies/Details/1` | HTTP 200 | Details page displayed the `Diamond` record |
| `/Movies/Edit/1` | HTTP 200 | Edit page displayed the `Diamond` record |
| `/Movies/Delete/1` | HTTP 200 | Delete confirmation displayed the `Diamond` record |

The verification used GET requests only and did not add, edit, or delete database
records.

## Build note

The project previously completed Debug and Release builds with zero warnings and
zero errors. When the application is currently running, a second Debug build can
report that `week1HelloWorldMVC.exe` is locked. Stop the running application with
`Ctrl+C` before rebuilding Debug output.
