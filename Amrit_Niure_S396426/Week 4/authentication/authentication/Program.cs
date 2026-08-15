using authentication.Auth;
using authentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("users-db")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync(Roles.Admin))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
    }
    if (!await roleManager.RoleExistsAsync(Roles.Member))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.Member));
    }
}

RegisterUser.MapEndPoint(app);
LoginUser.MapEndpoint(app);
app.UseHttpsRedirection();

app.Run();

