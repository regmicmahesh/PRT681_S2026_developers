namespace CleanApp.Domain.Common;

public interface IValidationResult
{
    Error[] Errors { get; }
}

/// <summary>
/// A <see cref="Result"/> that failed FluentValidation, carrying every validation
/// <see cref="Error"/> instead of just one. Produced by the Application layer's
/// MediatR ValidationBehavior and detected via <see cref="IValidationResult"/>.
/// </summary>
public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(false, Error.Validation("Validation.General", "One or more validation errors occurred.")) =>
        Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(default, false, Error.Validation("Validation.General", "One or more validation errors occurred.")) =>
        Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}
