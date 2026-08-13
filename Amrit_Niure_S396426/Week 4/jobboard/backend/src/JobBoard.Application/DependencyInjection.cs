using System.Reflection;
using FluentValidation;
using JobBoard.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, params Assembly[] additionalHandlerAssemblies)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblies = new[] { assembly }.Concat(additionalHandlerAssemblies).ToArray();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
