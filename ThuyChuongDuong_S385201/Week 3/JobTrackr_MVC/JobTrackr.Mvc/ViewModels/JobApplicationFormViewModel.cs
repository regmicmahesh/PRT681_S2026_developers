using System.ComponentModel.DataAnnotations;
using JobTrackr.Mvc.Enums;

namespace JobTrackr.Mvc.ViewModels;

public class JobApplicationFormViewModel
{
    [Required]
    [StringLength(150)]
    [Display(Name = "Company name")]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [Display(Name = "Job title")]
    public string JobTitle { get; set; } = string.Empty;

    [Url]
    [Display(Name = "Job advertisement URL")]
    public string? JobUrl { get; set; }

    [Display(Name = "Application status")]
    public ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Draft;

    [Display(Name = "Date applied")]
    public DateOnly? DateApplied { get; set; }

    [Range(0, 100_000_000)]
    [Display(Name = "Minimum salary")]
    public decimal? MinimumSalary { get; set; }

    [Range(0, 100_000_000)]
    [Display(Name = "Maximum salary")]
    public decimal? MaximumSalary { get; set; }

    [StringLength(3, MinimumLength = 3)]
    public string? Currency { get; set; } = "AUD";

    [Display(Name = "Salary period")]
    public SalaryPeriod? SalaryPeriod { get; set; } = Enums.SalaryPeriod.Yearly;
}
