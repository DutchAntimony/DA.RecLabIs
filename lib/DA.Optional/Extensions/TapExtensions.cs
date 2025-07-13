namespace DA.Optional.Extensions;

public static class TapExtensions
{
    /// <summary>
    /// Execute an action with the value of the option if a value is present.
    /// </summary>
    /// <param name="option">The option that the action should be performed on.</param>
    /// <param name="action">The action to perform.</param>
    public static void Tap<TValue>(this Option<TValue> option, Action<TValue> action)
    {
        if (option.TryGetValue(out var value))
        {
            action(value);
        }
    }

    /// <summary>
    /// Execute an action with the value of the option if a value is present.
    /// </summary>
    /// <param name="option">The option on which the action should be applied.</param>
    /// <param name="action">The action to perform.</param>
    public static async Task TapAsync<TValue>(this Option<TValue> option, Func<TValue, Task> action)
    {
        if (option.TryGetValue(out var value))
        {
            await action(value);
        }
    }

    /// <summary>
    /// Execute an action with the value of the option if a value is present.
    /// </summary>
    /// <param name="optionTask">The task that will return an option on which the action should be applied.</param>
    /// <param name="action">The action to perform.</param>
    public static async Task Tap<TValue>(this Task<Option<TValue>> optionTask, Action<TValue> action) =>
        Tap(await optionTask, action);

    /// <summary>
    /// Execute an action with the value of the option if a value is present.
    /// </summary>
    /// <param name="optionTask">The task that will return an option on which the action should be applied.</param>
    /// <param name="action">The action to perform.</param>
    public static async Task TapAsync<TValue>(this Task<Option<TValue>> optionTask, Func<TValue, Task> action) =>
        await TapAsync(await optionTask, action);
}
