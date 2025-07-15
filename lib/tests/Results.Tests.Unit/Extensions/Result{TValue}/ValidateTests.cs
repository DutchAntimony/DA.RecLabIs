namespace Results.Tests.Unit.Extensions.ResultOfTValue;

public class ValidateTests : ExtensionTestsBase
{
    protected readonly Func<bool> _successPredicate = () => true;
    protected readonly Func<bool> _failurePredicate = () => false;
    protected readonly ValidationFailure _newFailure = new ValidationFailure("Test", "Validation failed");

    [Fact]
    public void Validate_Should_RemainOriginalResult_WhenResultIsSuccess_AndValidationPasses()
    {
        var result = _successResult.Validate(_successPredicate, _newFailure);
        result.ShouldBe(_successResult);
    }

    [Fact]
    public void Validate_Should_ReturnOriginalResult_WhenResultIsFailure()
    {
        var result = _failureResult.Validate(_successPredicate, _newFailure);
        result.ShouldBeFailureWithError(_originalError);
    }

    [Fact]
    public void Validate_Should_ReturnFailure_WhenResultIsSuccess_AndValidationFails()
    {
        var result = _successResult.Validate(_failurePredicate, _newFailure);
        result.TryGetFailure(out var error).ShouldBeTrue();
        if (error is not ValidationError validationError)
        {
            throw new InvalidOperationException("Expected a ValidationError error type.");
        }
        validationError.Failures.Count.ShouldBe(1);
        validationError.Failures.First().ShouldBe(_newFailure);
    }

    [Fact]
    public void Validate_Should_AddValidationFailure_WhenResultIsValidationFailure_AndValidationFails()
    {
        var originalValidationFailure = new ValidationFailure("Original", "Existing failure before test");
        Result validationErrorResult = new ValidationError(originalValidationFailure);

        var result = validationErrorResult.Validate(_failurePredicate, _newFailure);
        result.TryGetFailure(out var error).ShouldBeTrue();
        if (error is not ValidationError validationError)
        {
            throw new InvalidOperationException("Expected a ValidationError error type.");
        }
        validationError.Failures.Count.ShouldBe(2);
        validationError.Failures.First().ShouldBe(originalValidationFailure);
        validationError.Failures.Last().ShouldBe(_newFailure);
    }

    [Fact]
    public async Task ValidateAsyncOverloads()
    {
        Func<Task<bool>> asyncPredicate = async () => await Task.FromResult(true);
        (await _successResult.ValidateAsync(asyncPredicate, _newFailure)).IsSuccess.ShouldBeTrue();
        (await _failureResult.ValidateAsync(asyncPredicate, _newFailure)).ShouldBeFailureWithError(_originalError);
        (await _successResultTask.Validate(_successPredicate, _newFailure)).IsSuccess.ShouldBeTrue();
        (await _successResultTask.ValidateAsync(asyncPredicate, _newFailure)).IsSuccess.ShouldBeTrue();
    }
}