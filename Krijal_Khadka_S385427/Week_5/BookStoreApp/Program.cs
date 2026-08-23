using BookStoreApp.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services.
builder.Services.AddControllersWithViews();

// Add Razor Pages because the built-in Identity UI uses Razor Pages.
builder.Services.AddRazorPages();

// Configure Entity Framework Core and SQL Server.
builder.Services.AddDbContext<EfBookStoreContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

// Configure ASP.NET Core Identity.
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        // Email confirmation is disabled for this POC.
        options.SignIn.RequireConfirmedAccount = false;

        // Simple password rules for classroom testing.
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<EfBookStoreContext>();

var app = builder.Build();

// Insert the Book and Printer sample data.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<EfBookStoreContext>();

    DbInitializer.Seed(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Check whether the user is logged in.
app.UseAuthentication();

// Check whether the user has permission to access a resource.
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Map the Identity registration and login Razor Pages.
app.MapRazorPages();

app.Run();