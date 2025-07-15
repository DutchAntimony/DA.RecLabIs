using DA.Results.Errors;
using System.Diagnostics.CodeAnalysis;

namespace DA.Results;

/// <summary>
/// Monad for representing the result of an operation without a value.
/// The result monad can either be a success or a failure with an <see cref="Error"/>.
/// </summary>
public class Result : IResult
{
    private readonly Error? _error;

    ///<inheritdoc />
    public bool IsSuccess { get; }

    private Result(bool isSuccess, Error? error) =>
        (IsSuccess, _error) = (isSuccess, error);

    ///<inheritdoc />
    public bool TryGetFailure([MaybeNullWhen(false)] out Error error)
    {
        error = _error;
        return !IsSuccess;
    }

    /// <summary>
    /// Create a new Success result.
    /// </summary>
    /// <returns>New instance of a <see cref="Result"> that is a Success.</returns>
    public static Result Success() => new(true, null);

    /// <summary>
    /// Create a new Failure result with the provided value.
    /// </summary>
    /// <param name="error">The error encapsulated that causes the result to be a failure.</param>
    /// <returns>New instance of a <see cref="Result{TValue}"> with the provided error as error.</returns>
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => Result<TValue>.Success(value);

    public static implicit operator Result(Error error) => Failure(error);
}
