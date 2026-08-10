# JobTrackr MVC Only

This is a standalone ASP.NET Core MVC version of JobTrackr.

It is designed for the **Practice MVC** task only. It does not depend on a Web API or Blazor project.

## What it demonstrates

- Model
- View
- Controller
- Dependency Injection
- Repository interface
- In-memory repository
- Razor Views
- ViewModels
- Model binding
- Validation
- CRUD
- LINQ search/filter/sort
- Pagination

## Run

From the project root:

```bash
dotnet build JobTrackr.sln
dotnet run --project JobTrackr.Mvc/JobTrackr.Mvc.csproj
```

Then open:

```text
http://localhost:5091
```

## Fast learning order

1. `Program.cs`
2. `Models/JobApplication.cs`
3. `Repositories/IJobApplicationRepository.cs`
4. `Repositories/JobApplicationRepository.cs`
5. `Controllers/JobApplicationsController.cs`
6. `ViewModels/`
7. `Views/JobApplications/Index.cshtml`
8. `Views/JobApplications/Create.cshtml`

## MVC request flow

```text
Browser
  ↓
JobApplicationsController
  ↓
IJobApplicationRepository
  ↓
JobApplicationRepository
  ↓
List<JobApplication>
  ↓
Controller creates ViewModel
  ↓
Razor View (.cshtml)
  ↓
HTML returned to browser
```

## Important

This version uses an in-memory list. Data is lost when the application stops. That is intentional for the MVC practice task.
