using CleanApp.Application.Common.Behaviors;
using CleanApp.Domain.Common;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.Tests.Common.Behaviors;

public class ValidationBehaviorTests
{
    public sealed record TestCommand(string Name) : IRequest<Result>;

    public sealed record TestQuery(string Name) : IRequest<Result<string>>;

    public sealed class TestCommandValidator : AbstractValidator<TestCommand>
    {
        public TestCommandValidator() => RuleFor(c => c.Name).NotEmpty();
    }

    public sealed class TestQueryValidator : AbstractValidator<TestQuery>
    {
        public TestQueryValidator() => RuleFor(c => c.Name).NotEmpty();
    }

    [Fact]
    public async Task Handle_WithNoValidators_CallsNext()
    {
        var behavior = new ValidationBehavior<TestCommand, Result>([]);
        var nextCalled = false;

        var result = await behavior.Handle(new TestCommand(""), _ =>
        {
            nextCalled = true;
            return Task.FromResult(Result.Success());
        }, CancellationToken.None);

        Assert.True(nextCalled);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsNext()
    {
        var behavior = new ValidationBehavior<TestCommand, Result>([new TestCommandValidator()]);

        var result = await behavior.Handle(
            new TestCommand("valid"), _ => Task.FromResult(Result.Success()), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WithInvalidCommand_ReturnsValidationResultWithoutCallingNext()
    {
        var behavior = new ValidationBehavior<TestCommand, Result>([new TestCommandValidator()]);
        var nextCalled = false;

        var result = await behavior.Handle(new TestCommand(""), _ =>
        {
            nextCalled = true;
            return Task.FromResult(Result.Success());
        }, CancellationToken.None);

        Assert.False(nextCalled);
        Assert.True(result.IsFailure);
        var validationResult = Assert.IsAssignableFrom<IValidationResult>(result);
        Assert.NotEmpty(validationResult.Errors);
    }

    [Fact]
    public async Task Handle_WithInvalidQuery_ReturnsGenericValidationResult()
    {
        var behavior = new ValidationBehavior<TestQuery, Result<string>>([new TestQueryValidator()]);

        var result = await behavior.Handle(
            new TestQuery(""), _ => Task.FromResult(Result.Success("ok")), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.IsType<ValidationResult<string>>(result);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
