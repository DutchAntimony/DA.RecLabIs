namespace DA.Results.Extensions;

public static class MapExtensions
{
    /// <summary>
    /// Maps the current <see cref="IResult"/> to a new <see cref="Result{TOut}"/> using the specified value.
    /// </summary>
    /// <typeparam name="TOut">The type of the value to map to.</typeparam>
    /// <param name="result">The current result to map from.</param>
    /// <param name="valueFunc">The function to generate the value to use for the mapping if the result does not represent a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> containing the specified value if the current result is successful; otherwise, the
    /// failure contained in the current result.</returns>
    public static Result<TOut> Map<TOut>(this IResult result, Func<TOut> valueFunc)
    {
        return result.TryGetFailure(out var error) ? error : valueFunc();
    }

    /// <summary>
    /// Maps the current <see cref="IResult"/> to a new <see cref="Result{TOut}"/> using the specified value.
    /// </summary>
    /// <typeparam name="TOut">The type of the value to map to.</typeparam>
    /// <param name="result">The current result to map from.</param>
    /// <param name="valueTask">The task to generate the value to use for the mapping if the result does not represent a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> containing the specified value if the current result is successful; otherwise, the
    /// failure contained in the current result.</returns>
    public static async Task<Result<TOut>> MapAsync<TOut>(this IResult result, Func<Task<TOut>> valueTask)
    {
        return result.TryGetFailure(out var error) ? error : await valueTask();
    }

    /// <summary>
    /// Maps the current <see cref="IResult"/> to a new <see cref="Result{TOut}"/> using the specified value.
    /// </summary>
    /// <typeparam name="TOut">The type of the value to map to.</typeparam>
    /// <param name="resultTask">The current result Task to map from.</param>
    /// <param name="valueFunc">The function to generate the value to use for the mapping if the result does not represent a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> containing the specified value if the current result is successful; otherwise, the
    /// failure contained in the current result.</returns>
    public static async Task<Result<TOut>> Map<TOut>(this Task<Result> resultTask, Func<TOut> valueFunc)
    {
        var result = await resultTask;
        return Map(result, valueFunc);
    }

    /// <summary>
    /// Maps the current <see cref="IResult"/> to a new <see cref="Result{TOut}"/> using the specified value.
    /// </summary>
    /// <typeparam name="TOut">The type of the value to map to.</typeparam>
    /// <param name="resultTask">The current result Task to map from.</param>
    /// <param name="valueTask">The task to generate the value to use for the mapping if the result does not represent a failure.</param>
    /// <returns>A <see cref="Result{TOut}"/> containing the specified value if the current result is successful; otherwise, the
    /// failure contained in the current result.</returns>
    public static async Task<Result<TOut>> MapAsync<TOut>(this Task<Result> resultTask, Func<Task<TOut>> valueTask)
    {
        var result = await resultTask;
        return await MapAsync(result, valueTask);
    }

    /// <summary>
    /// Transforms the value contained in the current <see cref="Result{TIn}"/> instance  using the specified mapping
    /// function, and returns a new <see cref="Result{TOut}"/>.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value contained in the original <see cref="Result{TIn}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the output value to be contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TIn}"/> instance to transform.</param>
    /// <param name="map">A function that maps the input value of type <typeparamref name="TIn"/> to an output value of type <typeparamref
    /// name="TOut"/>.</param>
    /// <returns>A new <see cref="Result{TOut}"/> containing the transformed value if the original <see cref="Result{TIn}"/> contains
    /// a value;  otherwise, the error from the original <see cref="Result{TIn}"/>.</returns>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> map)
    {
        return result.TryGetValue(out var value, out var error) ? map(value) : error;
    }

    /// <summary>
    /// Transforms the value contained in the current <see cref="Result{TIn}"/> instance  using the specified async mapping
    /// function, and returns a new <see cref="Result{TOut}"/>.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value contained in the original <see cref="Result{TIn}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the output value to be contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TIn}"/> instance to transform.</param>
    /// <param name="map">A function that maps the input value of type <typeparamref name="TIn"/> to an output value of type <typeparamref
    /// name="TOut"/>.</param>
    /// <returns>A new <see cref="Result{TOut}"/> containing the transformed value if the original <see cref="Result{TIn}"/> contains
    /// a value;  otherwise, the error from the original <see cref="Result{TIn}"/>.</returns>
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> map)
    {
        return result.TryGetValue(out var value, out var error) ? await map(value) : error;
    }

    /// <summary>
    /// Transforms the value contained in the current <see cref="Result{TIn}"/> instance  using the specified mapping
    /// function, and returns a new <see cref="Result{TOut}"/>.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value contained in the original <see cref="Result{TIn}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the output value to be contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result{TIn}"/> instance to transform.</param>
    /// <param name="map">A function that maps the input value of type <typeparamref name="TIn"/> to an output value of type <typeparamref
    /// name="TOut"/>.</param>
    /// <returns>A new <see cref="Result{TOut}"/> containing the transformed value if the original <see cref="Result{TIn}"/> contains
    /// a value;  otherwise, the error from the original <see cref="Result{TIn}"/>.</returns>
    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> map)
    {
        var result = await resultTask;
        return Map(result, map);
    }

    /// <summary>
    /// Transforms the value contained in the current <see cref="Result{TIn}"/> instance  using the specified async mapping
    /// function, and returns a new <see cref="Result{TOut}"/>.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value contained in the original <see cref="Result{TIn}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the output value to be contained in the resulting <see cref="Result{TOut}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result{TIn}"/> instance to transform.</param>
    /// <param name="map">A function that maps the input value of type <typeparamref name="TIn"/> to an output value of type <typeparamref
    /// name="TOut"/>.</param>
    /// <returns>A new <see cref="Result{TOut}"/> containing the transformed value if the original <see cref="Result{TIn}"/> contains
    /// a value;  otherwise, the error from the original <see cref="Result{TIn}"/>.</returns>
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<TOut>> map)
    {
        var result = await resultTask;
        return await MapAsync(result, map);
    }
}
