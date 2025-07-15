namespace Results.Tests.Unit.Extensions.Results;

public class MapTests : ExtensionTestsBase
{
    protected readonly int _value = 42;
    protected readonly Func<int> _successFunc;
    protected readonly Func<int> _forbiddenFunc = () => throw new InvalidOperationException("This function should not be called");
    public MapTests() : base()
    {
         _successFunc = () => _value;
    }

    [Fact]
    public void Map_Should_TakeValue_WhenResultIsSuccessAndNextIsSuccess()
    {
        var result = _successResult.Map(_successFunc);
        result.ShouldBeSuccessWithValue(42);
    }


    [Fact]
    public void Map_Should_ReturnOriginalError_WhenResultIsFailure()
    {
        var result = _failureResult.Map(_forbiddenFunc);
        result.ShouldBeFailureWithError(_originalError);
    }

    [Fact]
    public async Task MapAsyncOverloads()
    {
        Func<Task<int>> func = async () => await Task.FromResult(42);
        (await _successResult.MapAsync(func)).ShouldBeSuccessWithValue(42);
        (await _failureResult.MapAsync(func)).ShouldBeFailureWithError(_originalError);
        (await _successResultTask.Map(_successFunc)).ShouldBeSuccessWithValue(42);
        (await _successResultTask.MapAsync(func)).ShouldBeSuccessWithValue(42);
    }
}