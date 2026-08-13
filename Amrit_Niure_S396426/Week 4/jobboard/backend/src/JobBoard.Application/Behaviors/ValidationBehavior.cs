using System.Reflection;
using FluentValidation;
using JobBoard.Domain.Common;
using MediatR;

namespace JobBoard.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(cancellationToken);

        var failures = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Count == 0)
            return await next(cancellationToken);

        var error = new Error(
            "Validation.Failed",
            string.Join(" ", failures.Select(f => f.ErrorMessage)));

        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var args = typeof(TResponse).IsGenericType
            ? new object?[] { null, false, error }
            : new object?[] { false, error };

        var failureResult = Activator.CreateInstance(typeof(TResponse), flags, binder: null, args: args, culture: null);

        return (TResponse)failureResult!;
    }
}
