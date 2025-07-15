namespace Results.Tests.Unit.Extensions.ResultOfTValue;

public class BindTests : ExtensionTestsBase
{
    protected readonly Func<double, Result<int>> _successFunc = (value) => Result.Success((int)value);
    protected readonly Func<double, Result<int>> _failureFunc;
    protected readonly Func<double, Result<int>> _forbiddenFunc = (value) => throw new InvalidOperationException("This function should not be called");

    public BindTests() : base()
    {
        _failureFunc = (double val) => _nextError;
    }

    [Fact]
    public void BindT_Should_TakeValue_WhenResultIsSuccessAndNextIsSuccess()
    {
        var result = _successResult.Bind(_successFunc);
        result.ShouldBeSuccessWithValue<int>(3);
    }

    [Fact]
    public void BindT_Should_TakeError_WhenResultIsSuccessAndNextIsFailure()
    {
        var result = _successResult.Bind(_failureFunc);
        result.ShouldBeFailureWithError<int>(_nextError);
    }

    [Fact]
    public void BindT_Should_ReturnOriginalError_WhenResultIsFailure()
    {
        var result = _failureResult.Bind(_forbiddenFunc);
        result.ShouldBeFailureWithError<int>(_originalError);
    }

    [Fact]
    public async Task BindAsyncOverloads()
    {
        Func<double, Task<Result<int>>> func = async (value) => await Task.FromResult(Result.Success((int)value));
        (await _successResult.BindAsync(func)).ShouldBeSuccessWithValue<int>(3);
        (await _failureResult.BindAsync(func)).ShouldBeFailureWithError<int>(_originalError);

        (await _successResultTask.Bind(_successFunc)).ShouldBeSuccessWithValue<int>(3);
        (await _successResultTask.BindAsync(func)).ShouldBeSuccessWithValue<int>(3);
    }
}
