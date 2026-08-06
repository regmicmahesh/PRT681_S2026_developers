using CleanApp.Application.Common.Interfaces;

namespace CleanApp.Infrastructure.Services;

internal sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
