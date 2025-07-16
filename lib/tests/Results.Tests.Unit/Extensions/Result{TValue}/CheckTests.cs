namespace Results.Tests.Unit.Extensions.ResultOfTValue;

public class CheckTests : ExtensionTestsBase
{
    protected readonly Func<double, IResult> _successFunc = _ => Result.Success();
    protected readonly Func<double, IResult> _failureFunc;
    protected readonly Func<double, IResult> _forbiddenFunc = _ => throw new InvalidOperationException("This function should not be called");

    protected readonly Func<double, bool> _successPredicate = _ => true;
    protected readonly Func<double, bool> _failurePredicate;
    protected readonly Func<double, bool> _forbiddenPredicate = _ => throw new InvalidOperationException("This function should not be called");

    public CheckTests() : base()
    {
        _failureFunc = _ => Result.Failure(_nextError);
        _failurePredicate = _ => false;
    }

    [Fact]
    public void Check_Should_RemainUntouched_WhenResultIsSuccessAndNextIsSuccess()
    {
        var result = _successResult.Check(_successFunc);
        result.ShouldBe(_successResult);
    }

    [Fact]
    public void Check_Should_TakeError_WhenResultIsSuccessAndNextIsFailure()
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
        Func<double, Task<Result>> successFuncTask = async _ => await Task.FromResult(Result.Success());
        Func<double, Task<Result>> failureFuncTask = async _ => await Task.FromResult(Result.Failure(_nextError));
        (await _successResult.CheckAsync(successFuncTask)).IsSuccess.ShouldBeTrue();
        (await _successResult.CheckAsync(failureFuncTask)).ShouldBeFailureWithError(_nextError);
        (await _failureResult.CheckAsync(successFuncTask)).ShouldBeFailureWithError(_originalError);

        (await _successResultTask.Check(_successFunc)).IsSuccess.ShouldBeTrue();
        (await _successResultTask.CheckAsync(successFuncTask)).IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Check_Should_RemainUntouched_WhenResultIsSuccessAndPredicatePasses()
    {
        var result = _successResult.Check(_successPredicate, _nextError);
        result.ShouldBe(_successResult);
    }

    [Fact]
    public void Check_Should_TakeException_WhenResultIsSuccessAndPredicateFails()
    {
        var result = _successResult.Check(_failurePredicate, _nextError);
        result.ShouldBeFailureWithError(_nextError);
    }

    [Fact]
    public void Check_Should_RemainUntouched_OnPredicate_WhenResultIsFailure()
    {
        var result = _failureResult.Check(_forbiddenPredicate, _nextError);
        result.ShouldBe(_failureResult);
    }

    [Fact]
    public async Task CheckAsyncPredicateOverloads()
    {
        Func<double, Task<bool>> successFuncTask = async _ => await Task.FromResult(true);
        Func<double, Task<bool>> failureFuncTask = async _ => await Task.FromResult(false);
        (await _successResult.CheckAsync(successFuncTask, _nextError)).IsSuccess.ShouldBeTrue();
        (await _successResult.CheckAsync(failureFuncTask, _nextError)).ShouldBeFailureWithError(_nextError);
        (await _failureResult.CheckAsync(successFuncTask, _nextError)).ShouldBeFailureWithError(_originalError);

        (await _successResultTask.Check(_successPredicate, _nextError)).IsSuccess.ShouldBeTrue();
        (await _successResultTask.CheckAsync(successFuncTask, _nextError)).IsSuccess.ShouldBeTrue();
    }
}