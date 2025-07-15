using DA.Results.Errors;

namespace DA.Results.Extensions;

public static class MatchExtensions
{
    /// <summary>
    /// Matches the current <see cref="IResult"/> against the provided success and failure functions.
    /// </summary>
    /// <typeparam name="TOut">The type of the value to return if the result is successful.</typeparam>
    /// <param name="result">The current result to match against.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>The result of executing either the success or failure function based on the outcome of the current result.</returns>
    public static TOut Match<TOut>(this IResult result, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        if (result.TryGetFailure(out var error))
        {
            return onFailure(error);
        }
        return onSuccess();
    }

    /// <summary>
    /// Matches the current <see cref="IResult"/> against the provided success and failure tasks.
    /// </summary>
    /// <typeparam name="TOut">The type of the value to return if the result is successful.</typeparam>
    /// <param name="result">The current result to match against.</param>
    /// <param name="onSuccessAsync">The task to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The task to execute if the result is a failure.</param>
    /// <returns>The result of executing either the success or failure tasks based on the outcome of the current result.</returns>
    public static async Task<TOut> MatchAsync<TOut>(this IResult result, Func<Task<TOut>> onSuccessAsync, Func<Error, Task<TOut>> onFailureAsync)
    {
        if (result.TryGetFailure(out var error))
        {
            return await onFailureAsync(error);
        }
        return await onSuccessAsync();
    }

    /// <summary>
    /// Matches the current <see cref="IResult"/> against the provided success and failure functions.
    /// </summary>
    /// <typeparam name="TOut">The type of the value to return if the result is successful.</typeparam>
    /// <param name="resultTask">The current Task of result to match against.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>The result of executing either the success or failure function based on the outcome of the current result.</returns>
    public static async Task<TOut> Match<TOut>(this Task<Result> resultTask, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        var result = await resultTask;
        return Match(result, onSuccess, onFailure);
    }

    /// <summary>
    /// Matches the current <see cref="IResult"/> against the provided success and failure tasks.
    /// </summary>
    /// <typeparam name="TOut">The type of the value to return if the result is successful.</typeparam>
    /// <param name="resultTask">The current Task of result to match against.</param>
    /// <param name="onSuccessAsync">The task to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The task to execute if the result is a failure.</param>
    /// <returns>The result of executing either the success or failure tasks based on the outcome of the current result.</returns>
    public static async Task<TOut> MatchAsync<TOut>(this Task<Result> resultTask, Func<Task<TOut>> onSuccessAsync, Func<Error, Task<TOut>> onFailureAsync)
    {
        var result = await resultTask;
        return await MatchAsync(result, onSuccessAsync, onFailureAsync);
    }

    /// <summary>
    /// Matches the current <see cref="Result{TValue}"/> against the provided success and failure functions.
    /// </summary>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the value to return if the result is successful.</typeparam>
    /// <param name="result">The current result to match against.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>The result of executing either the success or failure function based on the outcome of the current result.</returns>
    public static TOut Match<TValue, TOut>(this Result<TValue> result, Func<TValue, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        if (result.TryGetValue(out var value, out var error))
        {
            return onSuccess(value);
        }
        return onFailure(error);
    }

    /// <summary>
    /// Matches the current <see cref="Result{TValue}"/> against the provided success and failure tasks.
    /// </summary>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the value to return if the result is successful.</typeparam>
    /// <param name="result">The current result to match against.</param>
    /// <param name="onSuccessAsync">The tasks to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The tasks to execute if the result is a failure.</param>
    /// <returns>The result of executing either the success or failure tasks based on the outcome of the current result.</returns>
    public static async Task<TOut> MatchAsync<TValue, TOut>(this Result<TValue> result, Func<TValue, Task<TOut>> onSuccessAsync, Func<Error, Task<TOut>> onFailureAsync)
    {
        if (result.TryGetValue(out var value, out var error))
        {
            return await onSuccessAsync(value);
        }
        return await onFailureAsync(error);
    }

    /// <summary>
    /// Matches the current <see cref="Result{TValue}"/> against the provided success and failure functions.
    /// </summary>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the value to return if the result is successful.</typeparam>
    /// <param name="resultTask">The current Task of result to match against.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>The result of executing either the success or failure function based on the outcome of the current result.</returns>
    public static async Task<TOut> Match<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        var result = await resultTask;
        return Match(result, onSuccess, onFailure);
    }

    /// <summary>
    /// Matches the current <see cref="Result{TValue}"/> against the provided success and failure tasks.
    /// </summary>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the value to return if the result is successful.</typeparam>
    /// <param name="resultTask">The current Task of result to match against.</param>
    /// <param name="onSuccessAsync">The tasks to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The tasks to execute if the result is a failure.</param>
    /// <returns>The result of executing either the success or failure tasks based on the outcome of the current result.</returns>
    public static async Task<TOut> MatchAsync<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TOut>> onSuccessAsync, Func<Error, Task<TOut>> onFailureAsync)
    {
        var result = await resultTask;
        return await MatchAsync(result, onSuccessAsync, onFailureAsync);
    }
}
