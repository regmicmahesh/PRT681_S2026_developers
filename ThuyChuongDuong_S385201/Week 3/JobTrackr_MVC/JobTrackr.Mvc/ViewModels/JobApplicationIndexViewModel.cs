using JobTrackr.Mvc.Enums;
using JobTrackr.Mvc.Models;

namespace JobTrackr.Mvc.ViewModels;

public class JobApplicationIndexViewModel
{
    public IReadOnlyList<JobApplication> Items { get; init; } = [];

    public string? Search { get; init; }

    public ApplicationStatus? Status { get; init; }

    public string SortBy { get; init; } = "created";

    public string SortDirection { get; init; } = "desc";

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 5;

    public int TotalRecords { get; init; }

    public int TotalPages =>
        TotalRecords == 0
            ? 0
            : (int)Math.Ceiling(TotalRecords / (double)PageSize);

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;
}
