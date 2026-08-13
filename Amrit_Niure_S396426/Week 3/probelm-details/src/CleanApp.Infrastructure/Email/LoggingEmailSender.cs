using CleanApp.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CleanApp.Infrastructure.Email;

/// <summary>Dev-mode fallback used when SMTP isn't configured, so the app runs out of the box.</summary>
internal sealed class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender
{
    public Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[DEV EMAIL] To: {To} | Subject: {Subject}\n{Body}", to, subject, htmlBody);
        return Task.CompletedTask;
    }
}
