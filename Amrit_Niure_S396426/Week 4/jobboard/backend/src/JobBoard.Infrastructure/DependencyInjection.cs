using JobBoard.Infrastructure.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, LoggingEmailSender>();

        return services;
    }
}
