namespace DA.Results.Errors;

/// <summary>
/// Abstract base class for representing errors in the DA.Results library.
/// </summary>
/// <param name="Message">Description of the error.</param>
public abstract record Error(string Message);