namespace DA.Results.Errors;

/// <summary>
/// Represents an error that occurred while processing a file.
/// </summary>
/// <remarks>This type encapsulates details about a file-related error, including the file path, the reason for
/// the error,  and an optional exception providing additional context. It is intended to be used for error reporting
/// and logging.</remarks>
/// <param name="FilePath">The path of the file that was processed.</param>
/// <param name="Reason">Description of why the error occured.</param>
/// <param name="Exception">Optional: the exception that was thrown when processing this file.</param>
public sealed record FileError(string FilePath, string Reason, Exception? Exception = null) : Error($"File error: {Reason} at '{FilePath}'");

