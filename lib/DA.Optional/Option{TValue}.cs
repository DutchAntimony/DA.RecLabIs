using System.Diagnostics.CodeAnalysis;

namespace DA.Optional;

/// <summary>
/// Represents an optional value that may or may not be present.
/// </summary>
/// <remarks>The <see cref="Option{TValue}"/> type is a lightweight alternative to nullable types or <see
/// cref="System.Nullable{T}"/>  for representing optional values. It provides methods and operators for safely handling
/// the presence or absence of a value. Use <see cref="Some(TValue)"/> to create an instance with a value, and <see
/// cref="None"/> to create an instance without a value.</remarks>
/// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
public sealed class Option<TValue> : IEquatable<Option<TValue>>, IEquatable<object>
{
    /// <summary>
    /// The value of the option if it is present, or null if it is not.
    /// </summary>
    private readonly TValue? _value;

    /// <summary>
    /// Boolean indicating whether the option has a value.
    /// </summary>
    public bool HasValue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Option{TValue}"/> class with the provided value.
    /// </summary>
    /// <param name="value">The value of the option.</param>
    public static Option<TValue> Some(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return new Option<TValue>(value, true);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Option{TValue}"/> class with no value.
    /// </summary>
    /// <returns></returns>
    public static Option<TValue> None() => new(default, false);

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="TValue"/> to an <see cref="Option{TValue}"/> instance.
    /// </summary>
    /// <remarks>This operator allows seamless conversion of a <typeparamref name="TValue"/> to an <see
    /// cref="Option{TValue}"/>. It is useful for scenarios where optional values are represented using the <see
    /// cref="Option{TValue}"/> type.</remarks>
    /// <param name="value">The value to be wrapped in an <see cref="Option{TValue}"/>. Cannot be null.</param>
    public static implicit operator Option<TValue>(TValue value) => Some(value);

    /// <summary>
    /// Implicitly converts a an <see cref="Option{NoContent}"/> instance to an <see cref="Option{TValue}"/> instance.
    /// </summary>
    /// <remarks>This operator allows seamless conversion from <see cref="Option{NoContent}"/> to <see cref="Option{TValue}"/>.
    /// It is useful for creating an <see cref="Option{TValue}"/> from <see cref="Option.None"/> without providing a <typeparamref name="TValue"/></remarks>
    /// <param name="value">The value to be wrapped in an <see cref="Option{TValue}"/>. Cannot be null.</param>
    public static implicit operator Option<TValue>(Option<NoContent> _) => None();

    /// <summary>
    /// Private constructor such that the option can only be created through the static methods.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="hasValue"></param>
    private Option(TValue? value, bool hasValue) => (_value, HasValue) = (value, hasValue);

    /// <summary>
    /// Attempts to retrieve the value stored in the current instance.
    /// </summary>
    /// <remarks>This method is useful for safely accessing the value of the instance without throwing an
    /// exception. If the instance does not contain a value, the <paramref name="value"/> parameter will be set to <see
    /// langword="null"/>.</remarks>
    /// <param name="value">When this method returns <see langword="true"/>, contains the value stored in the instance. If the method
    /// returns <see langword="false"/>, the value is <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the instance contains a value; otherwise, <see langword="false"/>.</returns>
    public bool TryGetValue([NotNullWhen(true)]out TValue? value)
    {
        value = _value!;
        return HasValue;
    }

    /// <summary>
    /// Override the ToString method to return the objects ToString if the Option has a value, and an empty string if not.
    /// </summary>
    public override string ToString() => TryGetValue(out var value) ? value.ToString() ?? typeof(TValue).Name : string.Empty;

    #region IEquatable
    public static bool operator ==(Option<TValue> option, TValue value) =>
        option.TryGetValue(out var myValue) && myValue.Equals(value);
    public static bool operator !=(Option<TValue> option, TValue value) =>
        !option.TryGetValue(out var myValue) || !myValue.Equals(value);
    public static bool operator ==(Option<TValue> option, object other) => option.Equals(other);
    public static bool operator !=(Option<TValue> option, object other) => !(option == other);
    public static bool operator ==(Option<TValue> first, Option<TValue> second) => first.Equals(second);
    public static bool operator !=(Option<TValue> first, Option<TValue> second) => !(first == second);

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj switch
        {
            null => false,
            Option<TValue> other => Equals(other),
            TValue value => Equals(value),
            _ => false
        };
    }

    private bool Equals(TValue value)
    {
        return HasValue && (value?.Equals(_value) ?? false);
    }
    public bool Equals(Option<TValue>? other)
    {
        if (!HasValue && (other is null || !other.HasValue))
            return true;  // both have no value => are equal.
        if (!HasValue || other is null || !other.HasValue)
            return false; // one has value, other not => not equal.
        return EqualityComparer<TValue>.Default.Equals(_value, other._value);
    }

    public override int GetHashCode() => TryGetValue(out var value) ? value.GetHashCode() : 0;
    #endregion
}