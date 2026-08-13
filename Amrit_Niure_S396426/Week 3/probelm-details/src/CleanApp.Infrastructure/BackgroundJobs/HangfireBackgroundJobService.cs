using System.Linq.Expressions;
using CleanApp.Application.Common.Interfaces;
using Hangfire;

namespace CleanApp.Infrastructure.BackgroundJobs;

internal sealed class HangfireBackgroundJobService(IBackgroundJobClient jobClient) : IBackgroundJobService
{
    public string Enqueue<TJob>(Expression<Func<TJob, Task>> methodCall) =>
        jobClient.Enqueue(methodCall);

    public string Schedule<TJob>(Expression<Func<TJob, Task>> methodCall, TimeSpan delay) =>
        jobClient.Schedule(methodCall, delay);
}
