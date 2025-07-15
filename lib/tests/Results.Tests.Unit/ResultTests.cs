namespace Results.Tests.Unit;

public class ResultTests
{
    [Fact]
    public void Success_Should_CreateSuccessResult()
    {
        var result = Result.Success();
        result.IsSuccess.ShouldBeTrue();
        result.TryGetFailure(out var _).ShouldBeFalse();
    }

    [Fact]
    public void Failure_Should_CreateFailureResult()
    {
        var error = new UnexpectedError(new Exception("Test error"));
        var result = Result.Failure(error);
        result.IsSuccess.ShouldBeFalse();
        result.TryGetFailure(out var returnedError).ShouldBeTrue();
        returnedError.ShouldBe(error);
    }

    [Fact]
    public void Error_Should_ImplicitlyBeConvertedToAFailureResult()
    {
        var error = new UnexpectedError(new Exception("Test error"));
        Result result = error;
        result.IsSuccess.ShouldBeFalse();
        result.TryGetFailure(out var returnedError).ShouldBeTrue();
        returnedError.ShouldBe(error);
    }

    [Fact]
    public void SuccessWithValue_Should_BeCreatedUsingResultFactoryMethod()
    {
        var value = "Test Value";
        var result = Result.Success(value);
        result.IsSuccess.ShouldBeTrue();
    }
}