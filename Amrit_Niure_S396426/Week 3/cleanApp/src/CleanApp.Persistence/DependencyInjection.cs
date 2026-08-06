using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using CleanApp.Persistence.Identity;
using CleanApp.Persistence.Interceptors;
using CleanApp.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanApp.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ?? "Data Source=cleanapp.db";

        services.AddScoped<DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            options.UseSqlite(connectionString)
                .AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>()));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ITodoListRepository, TodoListRepository>();
        services.AddScoped<ITodoItemRepository, TodoItemRepository>();

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
