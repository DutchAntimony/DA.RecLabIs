namespace DA.Optional.Extensions;

public static class TapIfNoneExtensions
{
    /// <summary>
    /// Execute an action if the option has no value.
    /// </summary>
    /// <param name="option">This option that the action should be performed on.</param>
    /// <param name="action">The action to perform.</param>
    public static void TapIfNone<TValue>(this Option<TValue> option, Action action)
    {
        if (!option.HasValue)
        {
            action();
        }
    }

    /// <summary>
    /// Execute an action if the option has no value.
    /// </summary>
    /// <param name="option">This option on which the action should be applied.</param>
    /// <param name="action">The action to perform.</param>
    public static async Task TapIfNoneAsync<TValue>(this Option<TValue> option, Func<Task> action)
    {
        if (!option.HasValue)
        {
            await action();
        }
    }

    /// <summary>
    /// Execute an action if the option has no value.
    /// </summary>
    /// <param name="optionTask">This task that will return an option on which the action should be applied.</param>
    /// <param name="action">The action to perform.</param>
    public static async Task TapIfNone<TValue>(this Task<Option<TValue>> optionTask, Action action) =>
        TapIfNone(await optionTask, action);

    /// <summary>
    /// Execute an action if the option has no value.
    /// </summary>
    /// <param name="optionTask">This task that will return an option on which the action should be applied.</param>
    /// <param name="action">The action to perform.</param>
    public static async Task TapIfNoneAsync<TValue>(this Task<Option<TValue>> optionTask, Func<Task> action) =>
        await TapIfNoneAsync(await optionTask, action);
}