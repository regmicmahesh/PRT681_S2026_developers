using Microsoft.EntityFrameworkCore;
using week1HelloWorldMVC.Data;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();

// Read the SQL Server connection string from appsettings.json
var connectionString = builder.Configuration
    .GetConnectionString("MvcMovieContext")
    ?? throw new InvalidOperationException(
        "Connection string 'MvcMovieContext' was not found."
    );

// Register the Entity Framework database context
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlServer(connectionString)
);

var app = builder.Build();

// Configure the HTTP request pipeline
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

app.Run();