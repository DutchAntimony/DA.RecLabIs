namespace Results.Tests.Unit.Extensions.ResultOfTValue;

public class TapTests : ExtensionTestsBase
{
    protected int _successCount = 0;
    protected readonly Action<double> _successAction;
    protected readonly Action<double> _forbiddenAction = _ => throw new InvalidOperationException("This function should not be called");

    public TapTests() : base()
    {
        _successAction = _ => _successCount++;
    }

    [Fact]
    public void Tap_Should_ExecuteAction_WhenResultIsSuccess()
    {
        _successCount = 0;
        var result = _successResult.Tap(_successAction);
        result.ShouldBe(_successResult);
        _successCount.ShouldBe(1);
    }

    [Fact]
    public void Tap_Should_RemainUntouched_WhenResultIsFailure()
    {
        _successCount = 0;
        var result = _failureResult.Tap(_forbiddenAction);
        result.ShouldBe(_failureResult);
        _successCount.ShouldBe(0);
    }

    [Fact]
    public async Task TapAsyncOverloads()
    {
        _successCount = 0;
        (await _successResult.TapAsync(successFuncTask)).ShouldBe(_successResult);
        _successCount.ShouldBe(1);

        _successCount = 0;
        (await _failureResult.TapAsync(successFuncTask)).ShouldBe(_failureResult);
        _successCount.ShouldBe(0);

        _successCount = 0;
        (await _successResultTask.Tap(_successAction)).IsSuccess.ShouldBeTrue();
        _successCount.ShouldBe(1);

        _successCount = 0;
        (await _successResultTask.TapAsync(successFuncTask)).IsSuccess.ShouldBeTrue();
        _successCount.ShouldBe(1);

        async Task successFuncTask(double value)
        {
            _successAction(value);
            await Task.Yield();
        }
    }
}