using Application.TodoItems.Commands.CompleteTodoItem;
using Application.TodoItems.Commands.CreateTodoItem;
using Application.TodoItems.Commands.DeleteTodoItem;
using Application.TodoItems.Commands.ReopenTodoItem;
using Application.TodoItems.Commands.UpdateTodoItem;
using Application.TodoItems.Queries.GetTodoItemById;
using Application.TodoItems.Queries.GetTodoItems;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Contracts;

namespace Presentation.Endpoints;

public static class TodoItemEndpoints
{
    public static IEndpointRouteBuilder MapTodoItemEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/todos").WithTags("Todos");

        group.MapGet("/", async (ISender sender, bool? isCompleted, CancellationToken cancellationToken) =>
            Results.Ok(await sender.Send(new GetTodoItemsQuery(isCompleted), cancellationToken)));

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            Results.Ok(await sender.Send(new GetTodoItemByIdQuery(id), cancellationToken)));

        group.MapPost("/", async (CreateTodoItemCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var id = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/todos/{id}", id);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateTodoItemRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(
                new UpdateTodoItemCommand(id, request.Title, request.Description, request.Priority, request.DueDate),
                cancellationToken);
            return Results.NoContent();
        });

        group.MapPost("/{id:guid}/complete", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new CompleteTodoItemCommand(id), cancellationToken);
            return Results.NoContent();
        });

        group.MapPost("/{id:guid}/reopen", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new ReopenTodoItemCommand(id), cancellationToken);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteTodoItemCommand(id), cancellationToken);
            return Results.NoContent();
        });

        return app;
    }
}
