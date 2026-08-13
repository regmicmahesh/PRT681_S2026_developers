using CleanApp.Application.TodoItems.Commands;
using CleanApp.Application.TodoItems.Queries;
using CleanApp.Presentation.Common;
using CleanApp.Presentation.TodoItems.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanApp.Presentation.TodoItems;

[Route("api/todo-items")]
public sealed class TodoItemsController(ISender sender) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByList([FromQuery] Guid todoListId, [FromQuery] bool? isDone, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTodoItemsQuery(todoListId, isDone), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTodoItemByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new CreateTodoItemCommand(request.TodoListId, request.Title, request.Priority, request.Note, request.ReminderUtc),
            cancellationToken);

        if (result.IsFailure)
            return HandleFailure(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateTodoItemCommand(id, request.Title, request.Priority, request.Note, request.ReminderUtc),
            cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CompleteTodoItemCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/reopen")]
    public async Task<IActionResult> Reopen(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ReopenTodoItemCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteTodoItemCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
