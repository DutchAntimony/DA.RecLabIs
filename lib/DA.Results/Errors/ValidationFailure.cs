namespace DA.Results.Errors;

/// <summary>
/// Represents a validation failure, containing information about the property that failed validation and the associated
/// error message.
/// </summary>
/// <remarks>This type is typically used to encapsulate validation errors in scenarios where data integrity or
/// business rules are enforced. It provides a concise way to describe the property that caused the failure and the
/// reason for the failure.</remarks>
/// <param name="PropertyName">The name of the property that has an invalid value.</param>
/// <param name="ErrorMessage">Description of was is wrong with the value and how to fix it.</param>
public sealed record class ValidationFailure(string PropertyName, string ErrorMessage)
{
    public static implicit operator ValidationError(ValidationFailure failure) => new(failure);
    public override string ToString() => $"{PropertyName}: {ErrorMessage}";
}

