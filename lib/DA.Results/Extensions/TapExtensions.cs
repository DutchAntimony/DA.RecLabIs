namespace DA.Results.Extensions;

public static class TapExtensions
{
    /// <summary>
    /// Executes the specified action if the <see cref="Result"/> represents a successful outcome.
    /// </summary>
    /// <remarks>This method is typically used to perform side effects, such as logging or updating state,
    /// when the operation represented by the <see cref="Result"/> is successful. The original <see cref="Result"/> is
    /// returned unchanged, enabling fluent chaining of operations.</remarks>
    /// <param name="result">The <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="action">The action to execute if <paramref name="result"/> is successful. Must not be null.</param>
    /// <returns>The original <see cref="Result"/> instance, allowing for method chaining.</returns>
    public static Result Tap(this Result result, Action action)
    {
        if (result.IsSuccess)
        {
            action();
        }
        return result;
    }

    /// <summary>
    /// Executes the specified task if the <see cref="Result"/> represents a successful outcome.
    /// </summary>
    /// <remarks>This method is typically used to perform side effects, such as logging or updating state,
    /// when the operation represented by the <see cref="Result"/> is successful. The original <see cref="Result"/> is
    /// returned unchanged, enabling fluent chaining of operations.</remarks>
    /// <param name="result">The <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="task">The task to await if <paramref name="result"/> is successful. Must not be null.</param>
    /// <returns>The original <see cref="Result"/> instance, allowing for method chaining.</returns>
    public static async Task<Result> TapAsync(this Result result, Func<Task> task)
    {
        if (result.IsSuccess)
        {
            await task();
        }
        return result;
    }

    /// <summary>
    /// Executes the specified action if the <see cref="Result"/> represents a successful outcome.
    /// </summary>
    /// <remarks>This method is typically used to perform side effects, such as logging or updating state,
    /// when the operation represented by the <see cref="Result"/> is successful. The original <see cref="Result"/> is
    /// returned unchanged, enabling fluent chaining of operations.</remarks>
    /// <param name="resultTask">The Task of <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="action">The action to execute if <paramref name="result"/> is successful. Must not be null.</param>
    /// <returns>The original <see cref="Result"/> instance, allowing for method chaining.</returns>
    public static async Task<Result> Tap(this Task<Result> resultTask, Action action)
    {
        var result = await resultTask;
        return Tap(result, action);
    }

    /// <summary>
    /// Executes the specified task if the <see cref="Result"/> represents a successful outcome.
    /// </summary>
    /// <remarks>This method is typically used to perform side effects, such as logging or updating state,
    /// when the operation represented by the <see cref="Result"/> is successful. The original <see cref="Result"/> is
    /// returned unchanged, enabling fluent chaining of operations.</remarks>
    /// <param name="resultTask">The Task of <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="task">The task to await if <paramref name="result"/> is successful. Must not be null.</param>
    /// <returns>The original <see cref="Result"/> instance, allowing for method chaining.</returns>
    public static async Task<Result> TapAsync(this Task<Result> resultTask, Func<Task> task)
    {
        var result = await resultTask;
        return await TapAsync(result, task);
    }

    /// <summary>
    /// Executes the specified action if the <see cref="Result{TValue}"/> represents a successful outcome.
    /// </summary>
    /// <remarks>This method is typically used to perform side effects, such as logging or updating state,
    /// when the operation represented by the <see cref="Result{TValue}"/> is successful. The original <see cref="Result{TValue}"/> is
    /// returned unchanged, enabling fluent chaining of operations.</remarks>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TValue}"/> instance to evaluate. Must not be null.</param>
    /// <param name="action">The action to execute if <paramref name="result"/> is successful. Must not be null.</param>
    /// <returns>The original <see cref="Result{TValue}"/> instance, allowing for method chaining.</returns>
    public static Result<TValue> Tap<TValue>(this Result<TValue> result, Action<TValue> action)
    {
        if (result.TryGetValue(out var value))
        {
            action(value);
        }
        return result;
    }

    /// <summary>
    /// Executes the specified task if the <see cref="Result{TValue}"/> represents a successful outcome.
    /// </summary>
    /// <remarks>This method is typically used to perform side effects, such as logging or updating state,
    /// when the operation represented by the <see cref="Result{TValue}"/> is successful. The original <see cref="Result{TValue}"/> is
    /// returned unchanged, enabling fluent chaining of operations.</remarks>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TValue}"/> instance to evaluate. Must not be null.</param>
    /// <param name="task">The task to execute if <paramref name="result"/> is successful. Must not be null.</param>
    /// <returns>The original <see cref="Result{TValue}"/> instance, allowing for method chaining.</returns>
    public static async Task<Result<TValue>> TapAsync<TValue>(this Result<TValue> result, Func<TValue, Task> task)
    {
        if (result.TryGetValue(out var value))
        {
            await task(value);
        }
        return result;
    }

    /// <summary>
    /// Executes the specified action if the <see cref="Result{TValue}"/> represents a successful outcome.
    /// </summary>
    /// <remarks>This method is typically used to perform side effects, such as logging or updating state,
    /// when the operation represented by the <see cref="Result{TValue}"/> is successful. The original <see cref="Result{TValue}"/> is
    /// returned unchanged, enabling fluent chaining of operations.</remarks>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="action">The action to execute if <paramref name="result"/> is successful. Must not be null.</param>
    /// <returns>The original <see cref="Result{TValue}"/> instance, allowing for method chaining.</returns>
    public static async Task<Result<TValue>> Tap<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> action)
    {
        var result = await resultTask;
        return Tap(result, action);
    }

    /// <summary>
    /// Executes the specified task if the <see cref="Result{TValue}"/> represents a successful outcome.
    /// </summary>
    /// <remarks>This method is typically used to perform side effects, such as logging or updating state,
    /// when the operation represented by the <see cref="Result{TValue}"/> is successful. The original <see cref="Result{TValue}"/> is
    /// returned unchanged, enabling fluent chaining of operations.</remarks>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="task">The task to execute if <paramref name="resultTask"/> is successful. Must not be null.</param>
    /// <returns>The original <see cref="Result{TValue}"/> instance, allowing for method chaining.</returns>
    public static async Task<Result<TValue>> TapAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> task)
    {
        var result = await resultTask;
        return await TapAsync(result, task);
    }
}
