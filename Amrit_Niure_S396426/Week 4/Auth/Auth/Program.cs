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
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SameUserOrPermissionAuthorizationHandler>();
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
