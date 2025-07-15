namespace Results.Tests.Unit.Extensions.ResultOfTValue;

public class MapTests : ExtensionTestsBase
{
    protected readonly int _value = 3;
    protected readonly Func<double, int> _successFunc;
    protected readonly Func<double, int> _forbiddenFunc = _ => throw new InvalidOperationException("This function should not be called");
   
    public MapTests() : base()
    {
        _successFunc = val => (int)val;
    }

    [Fact]
    public void Map_Should_TakeValue_WhenResultIsSuccessAndNextIsSuccess()
    {
        var result = _successResult.Map(_successFunc);
        result.ShouldBeSuccessWithValue<int>(_value);
    }


    [Fact]
    public void Map_Should_ReturnOriginalError_WhenResultIsFailure()
    {
        var result = _failureResult.Map(_forbiddenFunc);
        result.ShouldBeFailureWithError<int>(_originalError);
    }

    [Fact]
    public async Task MapAsyncOverloads()
    {
        Func<double, Task<int>> func = async val => _successFunc(val);
        (await _successResult.MapAsync(func)).ShouldBeSuccessWithValue<int>(_value);
        (await _failureResult.MapAsync(func)).ShouldBeFailureWithError<int>(_originalError);
        (await _successResultTask.Map(_successFunc)).ShouldBeSuccessWithValue<int>(_value);
        (await _successResultTask.MapAsync(func)).ShouldBeSuccessWithValue<int>(_value);
    }
}