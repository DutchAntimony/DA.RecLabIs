using DA.Results.Errors;
using System.Diagnostics.CodeAnalysis;

namespace DA.Results;

/// <summary>
/// Monad for representing the result of an operation with a value.
/// The result monad can either be a success with a value or a failure with an <see cref="Error"/>.
/// </summary>
public class Result<TValue> : IResult
{
    private readonly TValue? _value;
    private readonly Error? _error;

    /// <summary>
    /// Is the result a success?
    /// </summary>
    public bool IsSuccess { get; }

    private Result(bool isSuccess, TValue? value, Error? error) =>
        (IsSuccess, _value, _error) = (isSuccess, value, error);

    /// <summary>
    /// Try to get the value of the result, as an out parameter.
    /// </summary>
    /// <param name="value">The value encapsulated by this result.</param>
    /// <returns>True if result is a success, false if not.</returns>
    public bool TryGetValue([NotNullWhen(true)] out TValue? value)
    {
        value = _value;
        return IsSuccess;
    }

    /// <summary>
    /// Try to get the value or the error of the result, both as out parameter.
    /// </summary>
    /// <param name="value">The value encapsulated by this result.</param>
    /// <param name="error">The error that occured that caused this result to be a failure</param>
    /// <returns>True if result is a success, false if not.</returns>
    public bool TryGetValue([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out Error error)
    {
        value = _value;
        error = _error;
        return IsSuccess;
    }

    /// <summary>
    /// Try to get the error of the result, as an out parameter.
    /// </summary>
    /// <param name="error">The error that occured that caused this result to be a failure</param>
    /// <returns>True if result is a failure, false if a success.</returns>
    public bool TryGetFailure([MaybeNullWhen(false)] out Error error)
    {
        error = _error;
        return !IsSuccess;
    }

    /// <summary>
    /// Create a new Success result with the provided value.
    /// </summary>
    /// <param name="value">The value encapsulated by the result</param>
    /// <returns>New instance of a <see cref="Result{TValue}"> with the provided value as value.</returns>
    public static Result<TValue> Success(TValue value) => new(true, value, null);

    /// <summary>
    /// Create a new Failure result with the provided value.
    /// </summary>
    /// <param name="error">The error encapsulated that causes the result to be a failure.</param>
    /// <returns>New instance of a <see cref="Result{TValue}"> with the provided error as error.</returns>
    public static Result<TValue> Failure(Error error) => new(false, default, error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
    public static implicit operator Result(Result<TValue> result) => result.TryGetFailure(out var error) ? Result.Failure(error) : Result.Success();
}
