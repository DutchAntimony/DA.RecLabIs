using System.Collections.Immutable;

namespace DA.Results.Errors;

/// <summary>
/// Represents an error that occurs during validation, containing a collection of validation failures.
/// </summary>
/// <remarks>This type encapsulates multiple validation failures that occur when validating input or data. It
/// provides a structured way to represent validation errors, making it easier to handle and report them.</remarks>
/// <param name="Failures">Collection of validation failures to solve before redoing the request.</param>
public sealed record ValidationError(ImmutableList<ValidationFailure> Failures) : Error("Validation failed")
{
    public ValidationError(params IEnumerable<ValidationFailure> failures) : this(failures.ToImmutableList()) { }
}

