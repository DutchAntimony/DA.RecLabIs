using DA.Optional.Extensions;

namespace DA.Optional.Collections;

public static class CollectionExtensions
{
    /// <summary>
    /// Returns all elements of the sequence that have a value.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to return the values of.</param>
    public static IEnumerable<TValue> Values<TValue>(this IEnumerable<Option<TValue>> source) => 
        source.Values(value => true);

    /// <summary>
    /// Returns all elements of the sequence that have a value.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to return the values of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    public static IEnumerable<TValue> Values<TValue>(this IEnumerable<Option<TValue>> source, Func<TValue, bool> predicate)
    {
        foreach (var option in source)
        {
            if (option.TryGetValue(out var value) && predicate(value))
            {
                yield return value;
            }
        }
    }

    /// <summary>
    /// <summary>Returns the first element of a sequence, or <see cref="Option.None"> if the sequence contains no elements.</summary>
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to return the first value of.</param>
    public static Option<TValue> FirstOrNone<TValue>(this IEnumerable<Option<TValue>> source) =>
        source.Where(option => option.HasValue).FirstOrDefault(Option.None);

    /// <summary>
    /// <summary>Returns the first element of a sequence, or <see cref="Option.None"> if the sequence contains no elements.</summary>
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to return the first value of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    public static Option<TValue> FirstOrNone<TValue>(this IEnumerable<Option<TValue>> source, Func<TValue, bool> predicate) =>
        source.Where(option => option.TryGetValue(out var value) && predicate(value)).FirstOrDefault(Option.None);

    /// <summary>
    /// <summary>Returns the first element of a sequence, or <see cref="Option.None"> if the sequence contains no elements.</summary>
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to return the first value of.</param>
    public static Option<TValue> FirstOrNone<TValue>(this IEnumerable<TValue> source) =>
        source.FirstOrDefault().AsOption();

    /// <summary>
    /// <summary>Returns the first element of a sequence, or <see cref="Option.None"> if the sequence contains no elements.</summary>
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to return the first value of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    public static Option<TValue> FirstOrNone<TValue>(this IEnumerable<TValue> source, Func<TValue, bool> predicate) =>
        source.FirstOrDefault(predicate).AsOption();

    /// <summary>
    /// Returns the amount of non-empty entries in this sequence.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to return the count of.</param>
    public static int CountValues<TValue>(this IEnumerable<Option<TValue>> source) =>
        source.Count(option => option.HasValue);

    /// <summary>
    /// Returns the amount of non-empty entries in this sequence.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to return the count of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    public static int CountValues<TValue>(this IEnumerable<Option<TValue>> source, Func<TValue, bool> predicate) =>
        source.Count(option => option.TryGetValue(out var value) && predicate(value));

    /// <summary>
    /// Returns true if there are any non-empty entries in the sequence.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to check if it has any values.</param>
    public static bool AnyValues<TValue>(this IEnumerable<Option<TValue>> source) =>
        source.Any(option => option.HasValue);

    /// <summary>
    /// Returns true if there are any non-empty entries in the sequence.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to check if it has any values.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    public static bool AnyValues<TValue>(this IEnumerable<Option<TValue>> source, Func<TValue, bool> predicate) =>
        source.Any(option => option.TryGetValue(out var value) && predicate(value));
}