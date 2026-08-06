using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TheaterAdmin.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });


builder.Services.AddOpenApi();

var connectionString =
    builder.Configuration.GetConnectionString(
        "TheaterAdminContext")
    ?? throw new InvalidOperationException(
        "Connection string 'TheaterAdminContext' was not found.");

builder.Services.AddDbContext<TheaterAdminContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("JQueryClientPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("JQueryClientPolicy");

app.UseAuthorization();

app.MapGet("/", () => new
{
    message = "TheaterAdmin API is running",
    endpoints = new
    {
        categories = "/api/categories",
        movies = "/api/movies",
        openApi = "/openapi/v1.json"
    }
});

app.MapControllers();

app.Run();