using JobTrackr.Mvc.Enums;
using JobTrackr.Mvc.Models;
using JobTrackr.Mvc.Repositories;
using JobTrackr.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackr.Mvc.Controllers;

public class JobApplicationsController : Controller
{
    private readonly IJobApplicationRepository _repository;

    public JobApplicationsController(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index(
        string? search,
        ApplicationStatus? status,
        string sortBy = "created",
        string sortDirection = "desc",
        int page = 1,
        int pageSize = 5)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 20);

        IEnumerable<JobApplication> query = _repository.GetAll();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();

            query = query.Where(job =>
                job.CompanyName.Contains(
                    term,
                    StringComparison.OrdinalIgnoreCase)
                ||
                job.JobTitle.Contains(
                    term,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (status.HasValue)
        {
            query = query.Where(
                job => job.ApplicationStatus == status.Value);
        }

        query = ApplySorting(query, sortBy, sortDirection);

        var totalRecords = query.Count();

        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new JobApplicationIndexViewModel
        {
            Items = items,
            Search = search,
            Status = status,
            SortBy = sortBy,
            SortDirection = sortDirection,
            Page = page,
            PageSize = pageSize,
            TotalRecords = totalRecords
        };

        return View(model);
    }

    public IActionResult Details(Guid id)
    {
        var job = _repository.GetById(id);

        if (job is null)
            return NotFound();

        return View(job);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new JobApplicationFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(JobApplicationFormViewModel model)
    {
        ValidateBusinessRules(model);

        if (!ModelState.IsValid)
            return View(model);

        var job = JobApplication.Create(
            model.CompanyName,
            model.JobTitle);

        ApplyForm(job, model);

        _repository.Add(job);

        TempData["Message"] = "Job application created.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var job = _repository.GetById(id);

        if (job is null)
            return NotFound();

        return View(ToForm(job));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(
        Guid id,
        JobApplicationFormViewModel model)
    {
        var job = _repository.GetById(id);

        if (job is null)
            return NotFound();

        ValidateBusinessRules(model);

        if (!ModelState.IsValid)
            return View(model);

        ApplyForm(job, model);

        _repository.Update(job);

        TempData["Message"] = "Job application updated.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(Guid id)
    {
        var job = _repository.GetById(id);

        if (job is null)
            return NotFound();

        return View(job);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        if (!_repository.Delete(id))
            return NotFound();

        TempData["Message"] = "Job application deleted.";

        return RedirectToAction(nameof(Index));
    }

    private static IEnumerable<JobApplication> ApplySorting(
        IEnumerable<JobApplication> query,
        string sortBy,
        string sortDirection)
    {
        var descending = string.Equals(
            sortDirection,
            "desc",
            StringComparison.OrdinalIgnoreCase);

        return sortBy.ToLowerInvariant() switch
        {
            "company" => descending
                ? query.OrderByDescending(j => j.CompanyName)
                : query.OrderBy(j => j.CompanyName),

            "title" => descending
                ? query.OrderByDescending(j => j.JobTitle)
                : query.OrderBy(j => j.JobTitle),

            "salary" => descending
                ? query.OrderByDescending(j => j.MinimumSalary ?? decimal.MinValue)
                : query.OrderBy(j => j.MinimumSalary ?? decimal.MaxValue),

            "status" => descending
                ? query.OrderByDescending(j => j.ApplicationStatus)
                : query.OrderBy(j => j.ApplicationStatus),

            _ => descending
                ? query.OrderByDescending(j => j.CreatedAt)
                : query.OrderBy(j => j.CreatedAt)
        };
    }

    private void ValidateBusinessRules(
        JobApplicationFormViewModel model)
    {
        if (model.ApplicationStatus != ApplicationStatus.Draft &&
            !model.DateApplied.HasValue)
        {
            ModelState.AddModelError(
                nameof(model.DateApplied),
                "Date applied is required when status is not Draft.");
        }

        if (model.DateApplied.HasValue &&
            model.DateApplied.Value >
            DateOnly.FromDateTime(DateTime.UtcNow))
        {
            ModelState.AddModelError(
                nameof(model.DateApplied),
                "Date applied cannot be in the future.");
        }

        if (model.MinimumSalary.HasValue &&
            model.MaximumSalary.HasValue &&
            model.MaximumSalary < model.MinimumSalary)
        {
            ModelState.AddModelError(
                nameof(model.MaximumSalary),
                "Maximum salary cannot be lower than minimum salary.");
        }
    }

    private static void ApplyForm(
        JobApplication job,
        JobApplicationFormViewModel model)
    {
        job.CompanyName = model.CompanyName.Trim();
        job.JobTitle = model.JobTitle.Trim();

        job.JobUrl = string.IsNullOrWhiteSpace(model.JobUrl)
            ? null
            : model.JobUrl.Trim();

        job.ApplicationStatus = model.ApplicationStatus;
        job.DateApplied = model.DateApplied;
        job.MinimumSalary = model.MinimumSalary;
        job.MaximumSalary = model.MaximumSalary;

        job.Currency = string.IsNullOrWhiteSpace(model.Currency)
            ? null
            : model.Currency.Trim().ToUpperInvariant();

        job.SalaryPeriod = model.SalaryPeriod;
        job.UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static JobApplicationFormViewModel ToForm(
        JobApplication job)
    {
        return new JobApplicationFormViewModel
        {
            CompanyName = job.CompanyName,
            JobTitle = job.JobTitle,
            JobUrl = job.JobUrl,
            ApplicationStatus = job.ApplicationStatus,
            DateApplied = job.DateApplied,
            MinimumSalary = job.MinimumSalary,
            MaximumSalary = job.MaximumSalary,
            Currency = job.Currency,
            SalaryPeriod = job.SalaryPeriod
        };
    }
}
