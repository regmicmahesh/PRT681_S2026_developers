using System.Text;
using CleanApp.Application;
using CleanApp.Application.Auth;
using CleanApp.Infrastructure;
using CleanApp.Infrastructure.Identity;
using CleanApp.Persistence;
using CleanApp.Presentation;
using CleanApp.Presentation.Auth;
using CleanApp.WebApi;
using Hangfire;
using HealthChecks.Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;

// Serilog's recommended two-stage setup: a minimal "bootstrap" logger captures
// startup failures (e.g. bad configuration) before the full pipeline is available.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting CleanApp.WebApi");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddApplication();
    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddPresentation();

    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
        .WithTracing(tracing => tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter())
        .WithMetrics(metrics => metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddConsoleExporter());

    var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
        ?? throw new InvalidOperationException("Missing 'Jwt' configuration section.");

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            // Keep claim types exactly as issued (e.g. "sub") instead of ASP.NET Core's
            // legacy remapping to long ClaimTypes.* URIs - matches what CurrentUserService
            // reads and avoids a common source of confusion with JWT claims.
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });

    builder.Services.AddAuthorization(options =>
        options.AddPolicy(AuthorizationPolicies.RequireAdmin, policy => policy.RequireRole(Roles.Admin)));

    // Liveness ("/health"): is the process itself alive - no dependency checks.
    // Readiness ("/health/ready"): can this instance actually serve traffic - checks the
    // database and the Hangfire job storage it depends on. This is the standard split for
    // load balancer / Kubernetes probes.
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>(name: "database", tags: ["ready"])
        .AddHangfire(_ => { }, name: "hangfire", tags: ["ready"]);

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

        await IdentitySeeder.SeedAsync(scope.ServiceProvider);
    }

    app.UseExceptionHandler();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseHangfireDashboard("/jobs");

    app.MapControllers();

    app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => false });
    app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    // HostAbortedException is thrown deliberately by WebApplicationFactory (used in
    // integration tests) right after Build() to stop before Run() - it must propagate,
    // not be swallowed here, or the test host fails with a misleading "entry point
    // exited without ever building an IHost" error.
    Log.Fatal(ex, "CleanApp.WebApi terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program;
