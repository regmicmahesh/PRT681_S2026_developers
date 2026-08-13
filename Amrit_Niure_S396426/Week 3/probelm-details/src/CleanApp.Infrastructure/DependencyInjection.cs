using CleanApp.Application.Common.Interfaces;
using CleanApp.Infrastructure.BackgroundJobs;
using CleanApp.Infrastructure.Email;
using CleanApp.Infrastructure.Services;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

        var emailEnabled = configuration.GetValue<bool>($"{EmailSettings.SectionName}:Enabled");
        if (emailEnabled)
            services.AddScoped<IEmailSender, SmtpEmailSender>();
        else
            services.AddScoped<IEmailSender, LoggingEmailSender>();

        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseInMemoryStorage());

        services.AddHangfireServer();

        services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();
        services.AddScoped<IReminderJob, ReminderJob>();
        services.AddScoped<ITodoItemNotificationJob, TodoItemNotificationJob>();

        return services;
    }
}
