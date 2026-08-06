using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

const string AngularDevCors = "AngularDevCors";

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularDevCors, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(AngularDevCors);

app.MapTodoEndpoints();

app.Run();
