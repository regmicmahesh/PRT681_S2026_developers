using JobBoard.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleFailure(Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("HandleFailure should only be called with a failed Result.");

        var statusCode = result.Error.Code.EndsWith("NotFound", StringComparison.Ordinal)
            ? StatusCodes.Status404NotFound
            : StatusCodes.Status400BadRequest;

        return Problem(
            statusCode: statusCode,
            title: result.Error.Code,
            detail: result.Error.Message);
    }
}
