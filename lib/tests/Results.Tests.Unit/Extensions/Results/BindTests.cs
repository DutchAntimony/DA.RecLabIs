namespace Results.Tests.Unit.Extensions.Results;

public class BindTests : ExtensionTestsBase
{
    protected readonly Func<Result<int>> _successFunc = () => 42;
    protected readonly Func<Result<int>> _failureFunc;
    protected readonly Func<Result<int>> _forbiddenFunc = () => throw new InvalidOperationException("This function should not be called");

    public BindTests() : base()
    {
         _failureFunc = () => _nextError;
    }

    [Fact]
    public void Bind_Should_TakeValue_WhenResultIsSuccessAndNextIsSuccess()
    {
        var result = _successResult.Bind(_successFunc);
        result.ShouldBeSuccessWithValue(42);
    }

    [Fact]
    public void Bind_Should_TakeError_WhenResultIsSuccessAndNextIsFailure()
    {
        var result = _successResult.Bind(_failureFunc);
        result.ShouldBeFailureWithError(_nextError);
    }

    [Fact]
    public void Bind_Should_ReturnOriginalError_WhenResultIsFailure()
    {
        var result = _failureResult.Bind(_forbiddenFunc);
        result.ShouldBeFailureWithError(_originalError);
    }

    [Fact]
    public async Task BindAsyncOverloads()
    {
        Func<Task<Result<int>>> func = async () => await Task.FromResult(Result.Success(42));
        (await _successResult.BindAsync(func)).ShouldBeSuccessWithValue(42);
        (await _failureResult.BindAsync(func)).ShouldBeFailureWithError(_originalError);

        (await _successResultTask.Bind(_successFunc)).ShouldBeSuccessWithValue(42);
        (await _successResultTask.BindAsync(func)).ShouldBeSuccessWithValue(42);
    }
}
