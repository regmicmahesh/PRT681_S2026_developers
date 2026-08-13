using CleanApp.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CleanApp.Presentation.Common;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : HandleFailure(result);

    protected IActionResult HandleResult(Result result) =>
        result.IsSuccess ? NoContent() : HandleFailure(result);

    protected IActionResult HandleFailure(Result result)
    {
        if (result is IValidationResult validationResult)
            return ValidationProblem(ToModelState(validationResult.Errors));

        var statusCode = result.Error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(title: result.Error.Code, detail: result.Error.Message, statusCode: statusCode);
    }

    private static ModelStateDictionary ToModelState(Error[] errors)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in errors)
            modelState.AddModelError(error.Code, error.Message);

        return modelState;
    }
}
