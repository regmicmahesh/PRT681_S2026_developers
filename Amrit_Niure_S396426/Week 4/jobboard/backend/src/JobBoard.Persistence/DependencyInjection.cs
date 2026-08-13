using JobBoard.Domain.Repositories;
using JobBoard.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoard.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("JobBoard")
            ?? throw new InvalidOperationException("Connection string 'JobBoard' was not found.");

        services.AddDbContext<JobBoardDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<JobBoardDbContext>());
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();

        return services;
    }
}
