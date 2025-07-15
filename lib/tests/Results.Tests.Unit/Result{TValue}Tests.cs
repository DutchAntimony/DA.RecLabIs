namespace Results.Tests.Unit;

public class ResultOfTValueTests
{
       [Fact]
    public void Success_Should_CreateSuccessResultWithValue()
    {
        var value = 42;
        var result = Result<int>.Success(value);

        result.IsSuccess.ShouldBeTrue();

        result.TryGetFailure(out var _).ShouldBeFalse();

        result.TryGetValue(out var returnedValue).ShouldBeTrue();
        returnedValue.ShouldBe(value);

        result.TryGetValue(out returnedValue, out var _).ShouldBeTrue();
        returnedValue.ShouldBe(value);
    }

    [Fact]
    public void Success_Should_ImplicitlyBeConstructedFromValue()
    {
        var value = 42;
        Result<int> result = value;

        result.IsSuccess.ShouldBeTrue();

        result.TryGetFailure(out var _).ShouldBeFalse();

        result.TryGetValue(out var returnedValue).ShouldBeTrue();
        returnedValue.ShouldBe(value);

        result.TryGetValue(out returnedValue, out var _).ShouldBeTrue();
        returnedValue.ShouldBe(value);
    }
    
    [Fact]
    public void Failure_Should_CreateFailureResultWithError()
    {
        var error = new UnexpectedError(new Exception("Test error"));
        var result = Result<int>.Failure(error);

        result.IsSuccess.ShouldBeFalse();

        result.TryGetFailure(out var returnedError).ShouldBeTrue();
        returnedError.ShouldBe(error);

        result.TryGetValue(out var _).ShouldBeFalse();

        result.TryGetValue(out var _, out returnedError).ShouldBeFalse();
        returnedError.ShouldBe(error);
    }

    [Fact]
    public void Failure_Should_BeImplicitlyConvertedFromError()
    {
        var error = new UnexpectedError(new Exception("Test error"));
        Result<int> result = error;

        result.IsSuccess.ShouldBeFalse();

        result.TryGetFailure(out var returnedError).ShouldBeTrue();
        returnedError.ShouldBe(error);

        result.TryGetValue(out var _).ShouldBeFalse();

        result.TryGetValue(out var _, out returnedError).ShouldBeFalse();
        returnedError.ShouldBe(error);
    }

    [Fact]
    public void ResultOfTValue_Should_BeConvertibleToResult()
    {
        Result<int> resultOfT = 42;
        Result result = resultOfT;
        result.IsSuccess.ShouldBeTrue();

        resultOfT = new UnexpectedError(new Exception("Test error"));
        result = resultOfT;
        result.IsSuccess.ShouldBeFalse();
    }
}