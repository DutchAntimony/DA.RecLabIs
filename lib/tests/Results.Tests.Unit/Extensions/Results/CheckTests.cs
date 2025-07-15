namespace Results.Tests.Unit.Extensions.Results;

public class CheckTests : ExtensionTestsBase
{
    protected readonly Func<IResult> _successFunc = () => Result.Success();
    protected readonly Func<IResult> _failureFunc;
    protected readonly Func<IResult> _forbiddenFunc = () => throw new InvalidOperationException("This function should not be called");

    public CheckTests() : base() 
    {
        _failureFunc = () => Result.Failure(_nextError);
    }

    [Fact]
    public void Check_Should_RemainUntouched_WhenResultIsSuccessAndNextIsSuccess()
    {
        var result = _successResult.Check(_successFunc);
        result.ShouldBe(_successResult);
    }

    [Fact]
    public void Check_Should_TakeException_WhenResultIsSuccessAndNextIsFailure()
    {
        var result = _successResult.Check(_failureFunc);
        result.ShouldBeFailureWithError(_nextError);
    }

    [Fact]
    public void Check_Should_RemainUntouched_WhenResultIsFailure()
    {
        var result = _failureResult.Check(_forbiddenFunc);
        result.ShouldBe(_failureResult);
    }

    [Fact]
    public async Task CheckAsyncOverloads()
    {
        Func<Task<IResult>> successFuncTask = async () => await Task.FromResult(Result.Success());
        Func<Task<IResult>> failureFuncTask = async () => await Task.FromResult(Result.Failure(_nextError));
        (await _successResult.CheckAsync(successFuncTask)).IsSuccess.ShouldBeTrue();
        (await _successResult.CheckAsync(failureFuncTask)).ShouldBeFailureWithError(_nextError);
        (await _failureResult.CheckAsync(successFuncTask)).ShouldBeFailureWithError(_originalError);

        (await _successResultTask.Check(_successFunc)).IsSuccess.ShouldBeTrue();
        (await _successResultTask.CheckAsync(successFuncTask)).IsSuccess.ShouldBeTrue();
    }
}
