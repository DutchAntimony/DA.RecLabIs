namespace Results.Tests.Unit.Extensions.ResultOfTValue;

public abstract class ExtensionTestsBase
{
    protected double value = 3.14;
    protected readonly Result<double> _successResult;
    protected readonly Task<Result<double>> _successResultTask;
    protected readonly Result<double> _failureResult;
    protected readonly Task<Result<double>> _failureResultTask;

    protected readonly Error _originalError = new DomainError("Original error");
    protected readonly Error _nextError = new DomainError("Next error");

    protected ExtensionTestsBase()
    {
        _successResult = value;
        _successResultTask = Task.FromResult(Result.Success(value));
        _failureResult = _originalError;
        _failureResultTask = Task.FromResult(Result<double>.Failure(_originalError));
    }
}
