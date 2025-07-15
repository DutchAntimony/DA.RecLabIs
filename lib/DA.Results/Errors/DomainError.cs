namespace DA.Results.Errors;

/// <summary>
/// Represents an error specific to the domain layer of the application.
/// </summary>
/// <remarks>This type is used to encapsulate domain-specific error information, typically for scenarios  where
/// business rules or domain logic result in an error condition. It extends the base <see cref="Error"/>  type to
/// provide additional semantic meaning within the domain context.</remarks>
/// <param name="Message">Description of the error.</param>
public sealed record DomainError(string Message) : Error(Message);

