namespace DA.Optional.Extensions;

public static class AsOptionExtensions
{
    /// <summary>
    /// Converts a nullable object to an <see cref="Option{TValue}"/>.
    /// </summary>
    public static Option<TValue> AsOption<TValue>(this TValue? value) =>
        value is not null ? value : Option.None;

    /// <summary>
    /// Converts a nullable object to an <see cref="Option{TValue}"/>.
    /// </summary>
    public static async Task<Option<TValue>> AsOption<TValue>(this Task<TValue?> task) =>
        AsOption(await task);
}