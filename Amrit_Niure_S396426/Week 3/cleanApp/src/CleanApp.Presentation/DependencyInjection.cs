using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CleanApp.Presentation;

public static class DependencyInjection
{
    public static IMvcBuilder AddPresentation(this IServiceCollection services) =>
        services.AddControllers()
            .AddApplicationPart(Assembly.GetExecutingAssembly());
}
