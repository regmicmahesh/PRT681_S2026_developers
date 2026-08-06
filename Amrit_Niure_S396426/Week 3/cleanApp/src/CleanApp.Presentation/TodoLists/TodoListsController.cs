using CleanApp.Application.TodoLists.Commands;
using CleanApp.Application.TodoLists.Queries;
using CleanApp.Presentation.Common;
using CleanApp.Presentation.TodoLists.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanApp.Presentation.TodoLists;

[Route("api/todo-lists")]
public sealed class TodoListsController(ISender sender) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? titleContains = null,
        [FromQuery] TodoListSortBy sortBy = TodoListSortBy.CreatedOnUtc,
        [FromQuery] bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetTodoListsQuery(page, pageSize, titleContains, sortBy, sortDescending), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTodoListByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoListRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateTodoListCommand(request.Title, request.Colour), cancellationToken);
        if (result.IsFailure)
            return HandleFailure(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Rename(Guid id, RenameTodoListRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RenameTodoListCommand(id, request.Title), cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/colour")]
    public async Task<IActionResult> ChangeColour(Guid id, ChangeTodoListColourRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ChangeTodoListColourCommand(id, request.Colour), cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteTodoListCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
