namespace Results.Tests.Unit.Extensions.Results;

public abstract class ExtensionTestsBase
{
    protected readonly Result _successResult = Result.Success();
    protected readonly Task<Result> _successResultTask = Task.FromResult(Result.Success());
    protected readonly Result _failureResult;
    protected readonly Task<Result> _failureResultTask;

    protected readonly Error _originalError = new DomainError("Original error");
    protected readonly Error _nextError = new DomainError("Next error");
    
    protected ExtensionTestsBase()
    {
        _failureResult = _originalError;
        _failureResultTask = Task.FromResult(Result.Failure(_originalError));
    }
}
