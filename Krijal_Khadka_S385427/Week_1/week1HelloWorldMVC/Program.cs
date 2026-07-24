using Microsoft.EntityFrameworkCore;
using week1HelloWorldMVC.Data;
using week1HelloWorldMVC.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration
    .GetConnectionString("MvcMovieContext")
    ?? throw new InvalidOperationException(
        "Connection string 'MvcMovieContext' was not found."
    );

builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlServer(connectionString)
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Create a service scope and seed the database.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        SeedData.Initialize(services);
    }
    catch (Exception exception)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogError(
            exception,
            "An error occurred while seeding the database."
        );
    }
}

app.Run();