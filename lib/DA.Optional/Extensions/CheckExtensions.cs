namespace DA.Optional.Extensions;

public static class CheckExtensions
{
    /// <summary>
    /// Apply a predicate to the option and only retain the value if the current value matches the predicate.
    /// </summary>
    /// <param name="option">The option this method is applied to.</param>
    /// <param name="predicate">The predicate to match against</param>
    public static Option<TValue> Check<TValue>(this Option<TValue> option, Func<TValue, bool> predicate) =>
        !option.TryGetValue(out var value) ? Option.None : predicate(value) ? option : Option.None;

    /// <summary>
    /// Apply a predicate to the option and only retain the value if the current value matches the predicate.
    /// </summary>
    /// <param name="option">The option this method is applied to.</param>
    /// <param name="predicate">The predicate to match against</param>
    public static async Task<Option<TValue>> CheckAsync<TValue>(this Option<TValue> option, Func<TValue, Task<bool>> predicate) =>
        !option.TryGetValue(out var value) ? Option.None : await predicate(value) ? option : Option.None;

    /// <summary>
    /// Apply a predicate to the option and only retain the value if the current value matches the predicate.
    /// </summary>
    /// <param name="optionTask">The Task{Option} this method is applied to.</param>
    /// <param name="predicate">The predicate to match against</param>
    public static async Task<Option<TValue>> Check<TValue>(this Task<Option<TValue>> optionTask, Func<TValue, bool> predicate) =>
        Check(await optionTask, predicate);

    /// <summary>
    /// Apply a predicate to the option and only retain the value if the current value matches the predicate.
    /// </summary>
    /// <param name="optionTask">The Task{Option} this method is applied to.</param>
    /// <param name="predicate">The predicate to match against</param>
    public static async Task<Option<TValue>> CheckAsync<TValue>(this Task<Option<TValue>> optionTask, Func<TValue, Task<bool>> predicate) =>
        await CheckAsync(await optionTask, predicate);
}
