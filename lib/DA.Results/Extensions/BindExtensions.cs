namespace DA.Results.Extensions;

/// <summary>
/// Bind: Convert a Result{TIn} to a Result{TOut} by supplying a Result{TOut}.
/// It returns:
/// - A failure with the original issue if the original result was a failure.
/// - A failure with the second issue if the second result was a failure.
/// - A success with the second value and the original IgnoreWarnings configuration.
/// </summary>
public static class BindExtensions
{
    /// <summary>
    /// Executes the specified function if the current result represents a success, and propagates the failure if the
    /// current result represents an error.
    /// </summary>
    /// <typeparam name="TOut">The type of the value contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="result">The current result to evaluate. Must not be null.</param>
    /// <param name="next">A function that produces the next <see cref="Result{TOut}"/> to return if the current result is successful. The
    /// function is not invoked if the current result represents a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> representing the outcome of the <paramref name="next"/> function if the current
    /// result is successful, or the propagated failure if the current result represents an error.</returns>
    public static Result<TOut> Bind<TOut>(this IResult result, Func<Result<TOut>> next)
    {
        if (result.TryGetFailure(out var error))
        {
            return Result<TOut>.Failure(error);
        }
        return next();
    }

    /// <summary>
    /// Executes the specified function if the current result represents a success, and propagates the failure if the
    /// current result represents an error.
    /// </summary>
    /// <typeparam name="TOut">The type of the value contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="result">The current result to evaluate. Must not be null.</param>
    /// <param name="next">A task that produces the next <see cref="Result{TOut}"/> to return if the current result is successful. The
    /// function is not invoked if the current result represents a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> representing the outcome of the <paramref name="next"/> function if the current
    /// result is successful, or the propagated failure if the current result represents an error.</returns>
    public static async Task<Result<TOut>> BindAsync<TOut>(this IResult result, Func<Task<Result<TOut>>> next)
    {
        if (result.TryGetFailure(out var error))
        {
            return Result<TOut>.Failure(error);
        }
        return await next();
    }

    /// <summary>
    /// Executes the specified function if the current result represents a success, and propagates the failure if the
    /// current result represents an error.
    /// </summary>
    /// <typeparam name="TOut">The type of the value contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="resultTask">The current Task of a result to evaluate. Must not be null.</param>
    /// <param name="next">A function that produces the next <see cref="Result{TOut}"/> to return if the current result is successful. The
    /// function is not invoked if the current result represents a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> representing the outcome of the <paramref name="next"/> function if the current
    /// result is successful, or the propagated failure if the current result represents an error.</returns>
    public static async Task<Result<TOut>> Bind<TOut>(this Task<Result> resultTask, Func<Result<TOut>> next)
    {
        var result = await resultTask;
        return Bind(result, next);
    }

    /// <summary>
    /// Executes the specified function if the current result represents a success, and propagates the failure if the
    /// current result represents an error.
    /// </summary>
    /// <typeparam name="TOut">The type of the value contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="resultTask">The current Task of a result to evaluate. Must not be null.</param>
    /// <param name="next">A function that produces the next <see cref="Result{TOut}"/> to return if the current result is successful. The
    /// function is not invoked if the current result represents a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> representing the outcome of the <paramref name="next"/> function if the current
    /// result is successful, or the propagated failure if the current result represents an error.</returns>
    public static async Task<Result<TOut>> BindAsync<TOut>(this Task<Result> resultTask, Func<Task<Result<TOut>>> next)
    {
        var result = await resultTask;
        return await BindAsync(result, next);
    }

    /// <summary>
    /// Executes the specified function if the current result represents a success, and propagates the failure if the
    /// current result represents an error.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the current <see cref="Result{TIn}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the value contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="result">The current result to evaluate. Must not be null.</param>
    /// <param name="next">A function that produces the next <see cref="Result{TOut}"/> to return if the current result is successful. The
    /// function is not invoked if the current result represents a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> representing the outcome of the <paramref name="next"/> function if the current
    /// result is successful, or the propagated failure if the current result represents an error.</returns>
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> next)
    {
        if (result.TryGetValue(out var value, out var error))
        {
            return next(value);
        }
        return Result<TOut>.Failure(error);
    }

    /// <summary>
    /// Executes the specified function if the current result represents a success, and propagates the failure if the
    /// current result represents an error.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the current <see cref="Result{TIn}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the value contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="result">The current result to evaluate. Must not be null.</param>
    /// <param name="next">A function that produces the next <see cref="Result{TOut}"/> to return if the current result is successful. The
    /// function is not invoked if the current result represents a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> representing the outcome of the <paramref name="next"/> function if the current
    /// result is successful, or the propagated failure if the current result represents an error.</returns>
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> next)
    {
        if (result.TryGetValue(out var value, out var error))
        {
            return await next(value);
        }
        return Result<TOut>.Failure(error);
    }

    /// <summary>
    /// Executes the specified function if the current result represents a success, and propagates the failure if the
    /// current result represents an error.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the current <see cref="Result{TIn}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the value contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="resultTask">The current Task of result to evaluate. Must not be null.</param>
    /// <param name="next">A function that produces the next <see cref="Result{TOut}"/> to return if the current result is successful. The
    /// function is not invoked if the current result represents a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> representing the outcome of the <paramref name="next"/> function if the current
    /// result is successful, or the propagated failure if the current result represents an error.</returns>
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Result<TOut>> next)
    {
        var result = await resultTask;
        return Bind(result, next);
    }

    /// <summary>
    /// Executes the specified function if the current result represents a success, and propagates the failure if the
    /// current result represents an error.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the current <see cref="Result{TIn}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the value contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="resultTask">The current Task of result to evaluate. Must not be null.</param>
    /// <param name="next">A function that produces the next <see cref="Result{TOut}"/> to return if the current result is successful. The
    /// function is not invoked if the current result represents a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> representing the outcome of the <paramref name="next"/> function if the current
    /// result is successful, or the propagated failure if the current result represents an error.</returns>
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<Result<TOut>>> next)
    {
        var result = await resultTask;
        return await BindAsync(result, next);
    }
}
