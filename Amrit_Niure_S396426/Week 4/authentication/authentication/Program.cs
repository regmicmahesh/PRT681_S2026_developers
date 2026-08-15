using authentication.Auth;
using authentication.Authorization;
using authentication.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("users-db")));

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

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrations();
    await app.SeedRolesAndPermissions();

}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

RegisterUser.MapEndPoint(app);
LoginUser.MapEndpoint(app);

app.MapGet("me", (ClaimsPrincipal user) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);
    var email = user.FindFirstValue(ClaimTypes.Email)
                ?? user.FindFirstValue(JwtRegisteredClaimNames.Email);

    var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

    //var allClaims = user.Claims
    //    .GroupBy(c => c.Type)
    //    .ToDictionary(g => g.Key, g => g.Select(c => c.Value).ToArray());

    return Results.Ok(new
    {
        UserId = userId,
        Email = email,
        Roles = roles,
        //AllClaims = allClaims
    });
}).RequireAuthorization(policy => policy.RequireRole(Roles.Member));


app.Run();

