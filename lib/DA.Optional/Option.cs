using System.ComponentModel;

namespace DA.Optional;

/// <summary>
/// Static methods to make working with the Option{T} easier.
/// </summary>
public static class Option
{
    /// <summary>
    /// Creates an <see cref="Option{NoContent}"/> instance representing the absence of a value.
    /// </summary>
    /// <returns>An <see cref="Option{NoContent}"/> instance with no value.</returns>
    public static Option<NoContent> None => Option<NoContent>.None();

    /// <summary>
    public static Option<TValue> Some<TValue>(TValue value) => Option<TValue>.Some(value);
}

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed record NoContent();