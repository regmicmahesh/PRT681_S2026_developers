# Week 1 — Developer (Primary) and Business Analyst (Secondary)

- Student: Shijian Zhu
- Student ID: S394861
- Primary role: Developer
- Secondary role: Business Analyst

## Contents

- `week1HelloWorldMVC/`: ASP.NET Core MVC application with HelloWorld pages and a
  LocalDB-backed Movie CRUD feature.
- `week1HelloWorldMVC.Tests/`: xUnit validation and controller tests.
- `ShijianZhu.Week1.slnx`: solution entry point for building and testing both projects.
- `Career_Research/`: Developer research, BA research, and a three-week learning plan.
- `BA_Notes/`: Initial product requirements for the Movie application.
- `Setup_Evidence/`: Environment and verification evidence.
- `timesheet_ShijianZhu_S394861.xlsx`: Six-hour individual Week 1 timesheet.

## MVC completion checklist

- [x] Created an ASP.NET Core MVC project.
- [x] Added `HelloWorldController` and its views.
- [x] Added a validated `Movie` model, including `Rating`.
- [x] Configured EF Core with SQL Server Express LocalDB.
- [x] Added Movies controller and Create, Read, Update, and Delete views.
- [x] Added movie title/genre search.
- [x] Added client-side and server-side validation.
- [x] Completed Details, Edit, and Delete flows.
- [x] Added automatic development database migration and sample data.
- [x] Added automated tests for Movie validation and filtering.

See `week1HelloWorldMVC/README.md` for setup and run instructions.

Run all builds and automated tests from this directory:

```powershell
dotnet test ShijianZhu.Week1.slnx --configuration Release
```
