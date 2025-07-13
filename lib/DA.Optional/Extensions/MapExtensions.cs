namespace DA.Optional.Extensions;

public static class MapExtensions
{
    /// <summary>
    /// Map the option of <typeparamref name="TIn"/> to an option of <typeparamref name="TOut"/> by providing a value selector function.
    /// </summary>
    /// <typeparam name="TIn">The type of the original optional value.</typeparam>
    /// <typeparam name="TOut">The type of the resulting optional value.</typeparam>
    /// <param name="option">The option this method is applied to.</param>
    /// <param name="selector">The function to convert from <typeparamref name="TIn"/> to <typeparamref name="TOut"/></param>
    /// <returns>An option of type <typeparamref name="TOut"/> that has a value depending on the original value and the result of the selector.</returns>
    public static Option<TOut> Map<TIn, TOut>(this Option<TIn> option, Func<TIn, TOut?> selector) =>
        option.TryGetValue(out var value) ? selector(value).AsOption() : Option.None;

    /// <summary>
    /// Map the option of <typeparamref name="TIn"/> to an option of <typeparamref name="TOut"/> by providing a value selector function.
    /// </summary>
    /// <typeparam name="TIn">The type of the original optional value.</typeparam>
    /// <typeparam name="TOut">The type of the resulting optional value.</typeparam>
    /// <param name="option">The option this method is applied to.</param>
    /// <param name="selectorTask">The function to convert from <typeparamref name="TIn"/> to <typeparamref name="TOut"/></param>
    /// <returns>An option of type <typeparamref name="TOut"/> that has a value depending on the original value and the result of the selector.</returns>
    public static async Task<Option<TOut>> MapAsync<TIn, TOut>(this Option<TIn> option, Func<TIn, Task<TOut?>> selectorTask) =>
        option.TryGetValue(out var value) ? await selectorTask(value).AsOption() : Option.None;

    /// <summary>
    /// Map the option of <typeparamref name="TIn"/> to an option of <typeparamref name="TOut"/> by providing a value selector function.
    /// </summary>
    /// <typeparam name="TIn">The type of the original optional value.</typeparam>
    /// <typeparam name="TOut">The type of the resulting optional value.</typeparam>
    /// <param name="optionTask">The task that will result in the option to convert.</param>
    /// <param name="selector">The function to convert from <typeparamref name="TIn"/> to <typeparamref name="TOut"/></param>
    /// <returns>A <see cref="Task"/>{<see cref="Option"/>{<typeparamref name="TOut"/>}} that has a value depending on the original value and the result of the selector.</returns>
    public static async Task<Option<TOut>> Map<TIn, TOut>(this Task<Option<TIn>> optionTask, Func<TIn, TOut?> selector) =>
        Map(await optionTask, selector);

    /// <summary>
    /// Map the option of <typeparamref name="TIn"/> to an option of <typeparamref name="TOut"/> by providing a value selector function.
    /// </summary>
    /// <typeparam name="TIn">The type of the original optional value.</typeparam>
    /// <typeparam name="TOut">The type of the resulting optional value.</typeparam>
    /// <param name="optionTask">The task that will result in the option to convert.</param>
    /// <param name="selectorTask">The function to convert from <typeparamref name="TIn"/> to <typeparamref name="TOut"/></param>
    /// <returns>An option of type <typeparamref name="TOut"/> that has a value depending on the original value and the result of the selector.</returns>
    public static async Task<Option<TOut>> MapAsync<TIn, TOut>(this Task<Option<TIn>> optionTask, Func<TIn, Task<TOut?>> selectorTask) =>
        await MapAsync(await optionTask, selectorTask);
}
