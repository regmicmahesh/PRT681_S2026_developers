using CleanApp.Domain.Common;

namespace CleanApp.Domain.Tests.Common;

public class ResultTests
{
    [Fact]
    public void Success_ReturnsSuccessfulResultWithNoError()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_ReturnsFailedResultWithGivenError()
    {
        var error = Error.Validation("Test.Error", "Something went wrong.");

        var result = Result.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_WithNoneError_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => Result.Failure(Error.None));
    }

    [Fact]
    public void GenericSuccess_ExposesValue()
    {
        var result = Result.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void GenericFailure_AccessingValueThrows()
    {
        var result = Result.Failure<int>(Error.Validation("Test.Error", "Bad value."));

        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void ImplicitConversion_FromValue_CreatesSuccessResult()
    {
        Result<string> result = "hello";

        Assert.True(result.IsSuccess);
        Assert.Equal("hello", result.Value);
    }
}
