using DA.Results.Errors;
using System.Diagnostics.CodeAnalysis;

namespace DA.Results;

public interface IResult
{
    /// <summary>
    /// Is the result a success?
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Try to get the error of the result, as an out parameter.
    /// </summary>
    /// <param name="error">The error that occurred that caused this result to be a failure</param>
    /// <returns>True if result is a failure, false if a success.</returns>
    bool TryGetFailure([MaybeNullWhen(false)] out Error error);
}
