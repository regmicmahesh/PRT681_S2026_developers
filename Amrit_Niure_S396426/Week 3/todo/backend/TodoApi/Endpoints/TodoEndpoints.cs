using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/todos").WithTags("Todos");

        group.MapGet("/", async (TodoDbContext db) =>
        {
            var todos = await db.TodoItems
                .OrderBy(t => t.CreatedAt)
                .Select(t => new TodoItemDto(t.Id, t.Title, t.IsComplete, t.CreatedAt, t.DueDate))
                .ToListAsync();
            return Results.Ok(todos);
        });

        group.MapGet("/{id:int}", async (int id, TodoDbContext db) =>
        {
            var todo = await db.TodoItems.FindAsync(id);
            return todo is null
                ? Results.NotFound()
                : Results.Ok(new TodoItemDto(todo.Id, todo.Title, todo.IsComplete, todo.CreatedAt, todo.DueDate));
        });

        group.MapPost("/", async (CreateTodoDto dto, TodoDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return Results.BadRequest("Title is required.");

            var todo = new TodoItem { Title = dto.Title, DueDate = dto.DueDate };
            db.TodoItems.Add(todo);
            await db.SaveChangesAsync();

            var result = new TodoItemDto(todo.Id, todo.Title, todo.IsComplete, todo.CreatedAt, todo.DueDate);
            return Results.Created($"/api/todos/{todo.Id}", result);
        });

        group.MapPut("/{id:int}", async (int id, UpdateTodoDto dto, TodoDbContext db) =>
        {
            var todo = await db.TodoItems.FindAsync(id);
            if (todo is null) return Results.NotFound();

            if (string.IsNullOrWhiteSpace(dto.Title))
                return Results.BadRequest("Title is required.");

            todo.Title = dto.Title;
            todo.IsComplete = dto.IsComplete;
            todo.DueDate = dto.DueDate;
            await db.SaveChangesAsync();

            return Results.Ok(new TodoItemDto(todo.Id, todo.Title, todo.IsComplete, todo.CreatedAt, todo.DueDate));
        });

        group.MapDelete("/{id:int}", async (int id, TodoDbContext db) =>
        {
            var todo = await db.TodoItems.FindAsync(id);
            if (todo is null) return Results.NotFound();

            db.TodoItems.Remove(todo);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
