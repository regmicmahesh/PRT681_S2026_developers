using System.Text;
using System.Text.Json.Serialization;
using JobBoard.Api;
using JobBoard.Api.Authorization;
using JobBoard.Application;
using JobBoard.Infrastructure;
using JobBoard.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

const string DevCorsPolicy = "DevCors";

builder.Services.AddApplication(typeof(JobBoard.Infrastructure.DependencyInjection).Assembly);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddCors(options => options.AddPolicy(DevCorsPolicy, policy =>
    policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()));

// This service issues no tokens of its own - it trusts JWTs signed by the Auth service, so the
// Issuer/Audience/SecretKey here must match that service's Jwt: config exactly.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters.ValidIssuer = builder.Configuration["Jwt:Issuer"];
        options.TokenValidationParameters.ValidAudience = builder.Configuration["Jwt:Audience"];
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!));
    });

builder.Services.AddAuthorization(options =>
{
    // Reads (job/company listings and details) stay anonymous - a job board is meant to be
    // publicly browsable. Only mutating endpoints require a permission; ownership on top of that
    // is enforced explicitly in the controllers via IAuthorizationService.AuthorizeAsync, since
    // attribute policies can't see a route/body value as a resource. See Authorization/ for the
    // full convention, ported from the Auth service.
    options.AddPolicy("RequireJobCreate", policy => policy.RequireAnyPermission(Permissions.JobCreate));
    options.AddPolicy("RequireJobUpdate", policy => policy.RequireAnyPermission(Permissions.JobUpdate, Permissions.JobDelete));
    options.AddPolicy("RequireJobApply", policy => policy.RequireAnyPermission(Permissions.JobApply));
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CompanyOwnerOrPermissionAuthorizationHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors(DevCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
