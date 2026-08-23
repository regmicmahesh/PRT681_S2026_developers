using Auth.Auth;
using Auth.Authorization;
using Auth.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("auth-db")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

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
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Named policies for the simple (non-resource-based) permission checks, used by
    // [Authorize(Policy = "...")] on controller actions. Resource-based ownership checks (e.g.
    // "edit your own user") can't be expressed declaratively as an attribute - those call
    // IAuthorizationService.AuthorizeAsync explicitly inside the action instead (see UsersController).
    options.AddPolicy("RequireUsersRead", policy => policy.RequireAnyPermission(Permissions.UsersRead));
    options.AddPolicy("RequireUsersDelete", policy => policy.RequireAnyPermission(Permissions.UsersDelete));
    options.AddPolicy("RequireUsersManageRoles", policy => policy.RequireAnyPermission(Permissions.UsersManageRoles));

    // Job board. job:create/update/delete are capability grants only (the whole Employer/Recruiter
    // role has them) - CompaniesController/JobsController additionally check CompanyOwnerOrPermissionRequirement
    // for anything scoped to one company, since attribute policies can't see a route/body value as
    // a resource. See Authorization/README.md.
    options.AddPolicy("RequireJobCreate", policy => policy.RequireAnyPermission(Permissions.JobCreate));
    options.AddPolicy("RequireJobUpdate", policy => policy.RequireAnyPermission(Permissions.JobUpdate));
    options.AddPolicy("RequireJobDelete", policy => policy.RequireAnyPermission(Permissions.JobDelete));
    options.AddPolicy("RequireJobApply", policy => policy.RequireAnyPermission(Permissions.JobApply));
    options.AddPolicy("RequireApplicationReadAny", policy => policy.RequireAnyPermission(Permissions.ApplicationReadAny, Permissions.ApplicationManage));
    options.AddPolicy("RequireApplicationManage", policy => policy.RequireAnyPermission(Permissions.ApplicationManage));
    options.AddPolicy("RequireApplicationReadOwn", policy => policy.RequireAnyPermission(Permissions.ApplicationReadOwn));
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SameUserOrPermissionAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CompanyOwnerOrPermissionAuthorizationHandler>();
builder.Services.AddScoped<RefreshTokenService>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
}
await app.ApplyMigrations();
if (app.Configuration.GetValue<bool>("Seeding:Enabled"))
{
    await app.SeedRolesAndPermissions();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
