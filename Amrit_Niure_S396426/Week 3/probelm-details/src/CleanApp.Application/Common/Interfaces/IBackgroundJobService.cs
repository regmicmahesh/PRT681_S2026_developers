using System.Linq.Expressions;

namespace CleanApp.Application.Common.Interfaces;

/// <summary>
/// Thin abstraction over the background job runner (Hangfire in Infrastructure), so
/// Application handlers never take a direct dependency on the job library.
/// </summary>
public interface IBackgroundJobService
{
    string Enqueue<TJob>(Expression<Func<TJob, Task>> methodCall);

    string Schedule<TJob>(Expression<Func<TJob, Task>> methodCall, TimeSpan delay);
}
