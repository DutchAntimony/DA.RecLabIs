namespace Results.Tests.Unit;

public class ErrorTests
{
    [Fact]
    public void DomainError_Should_CreateDomainError()
    {
        var message = "Domain error occurred";
        var error = new DomainError(message);
        error.Message.ShouldBe(message);
    }

    [Fact]
    public void FileError_Should_CreateFileError()
    {
        var filePath = "/path/to/file.txt";
        var reason = "File not found";
        var error = new FileError(filePath, reason);
        error.Message.ShouldBe($"File error: {reason} at '{filePath}'");
    }

    [Fact]
    public void UnexpectedError_Should_CreateUnexpectedError()
    {
        var exception = new Exception("Unexpected exception");
        var error = new UnexpectedError(exception);
        error.Message.ShouldBe($"An unexpected error occurred: {exception.Message}");
    }

    [Fact]
    public void NotFoundError_Should_CreateNotFoundError()
    {
        var type = typeof(string).Name;
        var searchDescription = "searching for a string";
        var error = new NotFoundError(type, searchDescription);
        error.Message.ShouldBe($"No results of '{type}' where found given {searchDescription}");
    }

    [Fact]
    public void ValidationFailure_Should_CreateValidationFailure()
    {
        var field = "Username";
        var message = "Username is required";
        var failure = new ValidationFailure(field, message);
        failure.ToString().ShouldBe($"{field}: {message}");
    }

    [Fact]
    public void ValidationError_Should_BeImplicitlyConvertedFromValidationFailure()
    {
        var field = "Email";
        var message = "Email is invalid";
        ValidationFailure failure = new(field, message);
        ValidationError error = failure;
        error.Failures.Single().PropertyName.ShouldBe(field);
        error.Failures.Single().ErrorMessage.ShouldBe(message);
    }

    [Fact]
    public void ValidationError_Should_CreateValidationError()
    {
        ValidationFailure failure = new("Email", "Email is invalid");
        ValidationError error = new(failure);
        error.Message.ShouldBe("Validation failed");
    }
}