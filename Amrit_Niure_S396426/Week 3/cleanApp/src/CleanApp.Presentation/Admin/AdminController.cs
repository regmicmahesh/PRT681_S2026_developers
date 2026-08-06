using CleanApp.Application.Admin.Queries;
using CleanApp.Presentation.Auth;
using CleanApp.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanApp.Presentation.Admin;

// Adds to (not replaces) the [Authorize] on ApiControllerBase: callers must be both
// authenticated AND satisfy the RequireAdmin policy.
[Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
[Route("api/admin/todo-lists")]
public sealed class AdminController(ISender sender) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTodoLists(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? titleContains = null,
        [FromQuery] string? ownerEmailContains = null,
        [FromQuery] AdminTodoListSortBy sortBy = AdminTodoListSortBy.CreatedOnUtc,
        [FromQuery] bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetAllTodoListsQuery(page, pageSize, titleContains, ownerEmailContains, sortBy, sortDescending),
            cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTodoListById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAdminTodoListByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }
}
