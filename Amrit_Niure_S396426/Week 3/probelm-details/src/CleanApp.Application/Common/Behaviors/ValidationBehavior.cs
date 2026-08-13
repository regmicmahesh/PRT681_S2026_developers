using CleanApp.Domain.Common;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.Common.Behaviors;

/// <summary>
/// Runs all FluentValidation validators for the request. On failure it returns a
/// <see cref="ValidationResult"/>/<see cref="ValidationResult{TValue}"/> (built via
/// reflection so it matches TResponse's exact Result/Result&lt;T&gt; shape) instead of
/// throwing, keeping error handling uniform with the Result pattern used everywhere else.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .Select(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Length == 0)
            return await next();

        return CreateValidationResult<TResponse>(errors);
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
            return (ValidationResult.WithErrors(errors) as TResult)!;

        var validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod("WithErrors")!
            .Invoke(null, [errors]);

        return (TResult)validationResult!;
    }
}
