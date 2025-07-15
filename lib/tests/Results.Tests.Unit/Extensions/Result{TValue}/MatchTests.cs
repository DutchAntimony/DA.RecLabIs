namespace Results.Tests.Unit.Extensions.ResultOfTValue;

public class MatchTests : ExtensionTestsBase
{
    protected readonly Func<double, bool> _onSuccessFunc = _ => true;
    protected readonly Func<Error, bool> _onFailureFunc = _ => false;
    
    [Fact]
    public void Match_Should_TakeSuccessFunc_WhenResultIsSuccess()
    {
        var result = _successResult.Match(_onSuccessFunc, _onFailureFunc);
        result.ShouldBeTrue();
    }

    [Fact]
    public void Match_Should_TakeFailureFunc_WhenResultIsFailure()
    {
        var result = _failureResult.Match(_onSuccessFunc, _onFailureFunc);
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsyncOverloads()
    {
        Func<double, Task<bool>> onSuccessFunc = async _ => await Task.FromResult(true);
        Func<Error, Task<bool>> onFailureFunc = async _ => await Task.FromResult(false);
        
        (await _successResult.MatchAsync(onSuccessFunc, onFailureFunc)).ShouldBeTrue();
        (await _failureResult.MatchAsync(onSuccessFunc, onFailureFunc)).ShouldBeFalse();
        
        (await _successResultTask.Match(_onSuccessFunc, _onFailureFunc)).ShouldBeTrue();
        (await _successResultTask.MatchAsync(onSuccessFunc, onFailureFunc)).ShouldBeTrue();
    }
}