namespace DA.Results.Errors;

/// <summary>
/// Represents an error that occurs unexpectedly during application execution.
/// This should always indicate an implementation issue.
/// </summary>
/// <remarks>This error encapsulates an <see cref="Exception"/> instance that provides details about the
/// unexpected failure. It is typically used to represent unhandled exceptions or unforeseen issues that disrupt normal
/// operation.</remarks>
/// <param name="Exception">The exception that was unexpected.</param>
public sealed record UnexpectedError(Exception Exception, string Details) : Error($"{Details}: {Exception.Message}")
{
    public UnexpectedError(Exception exception) 
        : this(exception, "An unexpected error occurred") { }
};

