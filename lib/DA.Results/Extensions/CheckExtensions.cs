using DA.Results.Errors;

namespace DA.Results.Extensions;

/// <summary>
/// Check:
/// Evaluate a condition against the current <see cref="Result"/> and returns a new <see cref="Result"/>  based on
/// the outcome of the provided check function.
/// </summary>
public static class CheckExtensions
{
    /// <summary>
    /// Evaluates a condition against the current <see cref="Result"/> and returns a new <see cref="Result"/>  based on
    /// the outcome of the provided check function.
    /// </summary>
    /// <remarks>This method allows chaining additional checks on a <see cref="Result"/> object. If the
    /// current result is  unsuccessful or the check function does not yield a failure, the original result is returned
    /// unchanged.  If the check function produces a failure, the failure is propagated as a new <see
    /// cref="Result"/>.</remarks>
    /// <param name="result">The current <see cref="Result"/> to evaluate.</param>
    /// <param name="check">A function that performs a validation or check and returns an <see cref="IResult"/>.</param>
    /// <returns>The original <see cref="Result"/> if it is successful or if the check function does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result"/> representing the failure from the check function.</returns>
    public static Result Check(this Result result, Func<IResult> check)
    {
        if (!result.IsSuccess || !check().TryGetFailure(out var checkFailure))
        {
            return result; 
        }
        return Result.Failure(checkFailure);
    }

    /// <summary>
    /// Evaluates a condition against the current <see cref="Result"/> and returns a new <see cref="Result"/>  based on
    /// the outcome of the provided check function.
    /// </summary>
    /// <remarks>This method allows chaining additional checks on a <see cref="Result"/> object. If the
    /// current result is  unsuccessful or the check function does not yield a failure, the original result is returned
    /// unchanged.  If the check function produces a failure, the failure is propagated as a new <see
    /// cref="Result"/>.</remarks>
    /// <param name="result">The current <see cref="Result"/> to evaluate.</param>
    /// <param name="check">A task that performs a validation or check and returns an <see cref="IResult"/>.</param>
    /// <returns>The original <see cref="Result"/> if it is successful or if the check function does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result"/> representing the failure from the check function.</returns>
    public static async Task<Result> CheckAsync(this Result result, Func<Task<IResult>> check)
    {
        if (!result.IsSuccess)
        {
            return result;
        }

        var checkResult = await check();
        if (checkResult.TryGetFailure(out var checkFailure))
        {
            return checkFailure;
        }

        return result;
    }

    /// <summary>
    /// Evaluates a condition against the current <see cref="Result"/> and returns a new <see cref="Result"/>  based on
    /// the outcome of the provided check function.
    /// </summary>
    /// <remarks>This method allows chaining additional checks on a <see cref="Result"/> object. If the
    /// current result is  unsuccessful or the check function does not yield a failure, the original result is returned
    /// unchanged.  If the check function produces a failure, the failure is propagated as a new <see
    /// cref="Result"/>.</remarks>
    /// <param name="resultTask">The current Task <see cref="Result"/> to evaluate.</param>
    /// <param name="check">A function that performs a validation or check and returns an <see cref="IResult"/>.</param>
    /// <returns>The original <see cref="Result"/> if it is successful or if the check function does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result"/> representing the failure from the check function.</returns>
    public static async Task<Result> Check(this Task<Result> resultTask, Func<IResult> check)
    {
        var result = await resultTask;
        return Check(result, check);
    }

    /// <summary>
    /// Evaluates a condition against the current <see cref="Result"/> and returns a new <see cref="Result"/>  based on
    /// the outcome of the provided check function.
    /// </summary>
    /// <remarks>This method allows chaining additional checks on a <see cref="Result"/> object. If the
    /// current result is  unsuccessful or the check function does not yield a failure, the original result is returned
    /// unchanged.  If the check function produces a failure, the failure is propagated as a new <see
    /// cref="Result"/>.</remarks>
    /// <param name="resultTask">The current Task of <see cref="Result"/> to evaluate.</param>
    /// <param name="check">A task that performs a validation or check and returns an <see cref="IResult"/>.</param>
    /// <returns>The original <see cref="Result"/> if it is successful or if the check function does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result"/> representing the failure from the check function.</returns>
    public static async Task<Result> CheckAsync(this Task<Result> resultTask, Func<Task<IResult>> check)
    {
        var result = await resultTask;
        return await CheckAsync(result, check);
    }

    /// <summary>
    /// Performs a validation check on the value contained within the <see cref="Result{TValue}"/> instance.
    /// </summary>
    /// <remarks>This method allows chaining validation checks on a <see cref="Result{TValue}"/> instance. If
    /// the <paramref name="result"/> does not contain a value, the method returns the original <paramref
    /// name="result"/> without invoking the <paramref name="check"/> function.</remarks>
    /// <typeparam name="TValue">The type of the value contained within the <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TValue}"/> instance to validate. Must not be null.</param>
    /// <param name="check">A function that performs the validation on the value. The function should return an <see cref="IResult"/>
    /// indicating the outcome of the validation.</param>
    /// <returns>The original <see cref="Result{TValue}"/> if the validation succeeds, or the failure result returned by the
    /// <paramref name="check"/> function if the validation fails.</returns>
    public static Result<TValue> Check<TValue>(this Result<TValue> result, Func<TValue, IResult> check)
    {
        if (!result.TryGetValue(out var value))
        {
            return result;
        }

        var checkResult = check(value);
        if (checkResult.TryGetFailure(out var checkFailure))
        {
            return checkFailure;
        }

        return result;
    }

    /// <summary>
    /// Performs a async validation check on the value contained within the <see cref="Result{TValue}"/> instance.
    /// </summary>
    /// <remarks>This method allows chaining validation checks on a <see cref="Result{TValue}"/> instance. If
    /// the <paramref name="result"/> does not contain a value, the method returns the original <paramref
    /// name="result"/> without invoking the <paramref name="check"/> function.</remarks>
    /// <typeparam name="TValue">The type of the value contained within the <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TValue}"/> instance to validate. Must not be null.</param>
    /// <param name="check">A function that performs the validation on the value. The function should return an <see cref="IResult"/>
    /// indicating the outcome of the validation.</param>
    /// <returns>The original <see cref="Result{TValue}"/> if the validation succeeds, or the failure result returned by the
    /// <paramref name="check"/> function if the validation fails.</returns>
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Result<TValue> result, Func<TValue, Task<IResult>> check)
    {
        if (!result.TryGetValue(out var value))
        {
            return result;
        }

        var checkResult = await check(value);
        if (checkResult.TryGetFailure(out var checkFailure))
        {
            return checkFailure;
        }

        return result;
    }

    /// <summary>
    /// Performs a validation check on the value contained within the <see cref="Result{TValue}"/> instance.
    /// </summary>
    /// <remarks>This method allows chaining validation checks on a <see cref="Result{TValue}"/> instance. If
    /// the <paramref name="resultTask"/> does not contain a value, the method returns the original <paramref
    /// name="resultTask"/> without invoking the <paramref name="check"/> function.</remarks>
    /// <typeparam name="TValue">The type of the value contained within the <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result{TValue}"/> instance to validate. Must not be null.</param>
    /// <param name="check">A function that performs the validation on the value. The function should return an <see cref="IResult"/>
    /// indicating the outcome of the validation.</param>
    /// <returns>The original <see cref="Result{TValue}"/> if the validation succeeds, or the failure result returned by the
    /// <paramref name="check"/> function if the validation fails.</returns>
    public static async Task<Result<TValue>> Check<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, IResult> check)
    {
        var result = await resultTask;
        return Check(result, check);
    }

    /// <summary>
    /// Performs a async validation check on the value contained within the <see cref="Result{TValue}"/> instance.
    /// </summary>
    /// <remarks>This method allows chaining validation checks on a <see cref="Result{TValue}"/> instance. If
    /// the <paramref name="resultTask"/> does not contain a value, the method returns the original <paramref
    /// name="resultTask"/> without invoking the <paramref name="check"/> function.</remarks>
    /// <typeparam name="TValue">The type of the value contained within the <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result{TValue}"/> instance to validate. Must not be null.</param>
    /// <param name="check">A function that performs the validation on the value. The function should return an <see cref="IResult"/>
    /// indicating the outcome of the validation.</param>
    /// <returns>The original <see cref="Result{TValue}"/> if the validation succeeds, or the failure result returned by the
    /// <paramref name="check"/> function if the validation fails.</returns>
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<IResult>> check)
    {
        var result = await resultTask;
        return await CheckAsync(result, check);
    }

    /// <summary>
    /// Evaluates a condition on the value contained within the <see cref="Result{TValue}"/> and returns either the
    /// original result or an error.
    /// </summary>
    /// <remarks>This method allows chaining validation checks on a <see cref="Result{TValue}"/>. If the
    /// result does not contain a value or the condition defined by <paramref name="check"/> evaluates to <see
    /// langword="true"/>, the original result is returned. If the condition evaluates to <see langword="false"/>, the
    /// specified <paramref name="error"/> is returned.</remarks>
    /// <typeparam name="TValue">The type of the value contained within the <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TValue}"/> instance to check.</param>
    /// <param name="check">A function that defines the condition to evaluate on the value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the condition fails.</param>
    /// <returns>The original <see cref="Result{TValue}"/> if the condition is met or the result does not contain a value;
    /// otherwise, the specified <paramref name="error"/>.</returns>
    public static Result<TValue> Check<TValue>(this Result<TValue> result, Func<TValue, bool> check, Error error)
    {
        return !result.TryGetValue(out var value) || check(value) ? result : error;
    }

    /// <summary>
    /// Evaluates a async condition on the value contained within the <see cref="Result{TValue}"/> and returns either the
    /// original result or an error.
    /// </summary>
    /// <remarks>This method allows chaining validation checks on a <see cref="Result{TValue}"/>. If the
    /// result does not contain a value or the condition defined by <paramref name="check"/> evaluates to <see
    /// langword="true"/>, the original result is returned. If the condition evaluates to <see langword="false"/>, the
    /// specified <paramref name="error"/> is returned.</remarks>
    /// <typeparam name="TValue">The type of the value contained within the <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TValue}"/> instance to check.</param>
    /// <param name="check">A function that defines the condition to evaluate on the value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the condition fails.</param>
    /// <returns>The original <see cref="Result{TValue}"/> if the condition is met or the result does not contain a value;
    /// otherwise, the specified <paramref name="error"/>.</returns>
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Result<TValue> result, Func<TValue, Task<bool>> check, Error error)
    {
        return !result.TryGetValue(out var value) || await check(value) ? result : error;
    }

    /// <summary>
    /// Evaluates a condition on the value contained within the <see cref="Result{TValue}"/> and returns either the
    /// original result or an error.
    /// </summary>
    /// <remarks>This method allows chaining validation checks on a <see cref="Result{TValue}"/>. If the
    /// result does not contain a value or the condition defined by <paramref name="check"/> evaluates to <see
    /// langword="true"/>, the original result is returned. If the condition evaluates to <see langword="false"/>, the
    /// specified <paramref name="error"/> is returned.</remarks>
    /// <typeparam name="TValue">The type of the value contained within the <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result{TValue}"/> instance to check.</param>
    /// <param name="check">A function that defines the condition to evaluate on the value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the condition fails.</param>
    /// <returns>The original <see cref="Result{TValue}"/> if the condition is met or the result does not contain a value;
    /// otherwise, the specified <paramref name="error"/>.</returns>
    public static async Task<Result<TValue>> Check<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> check, Error error)
    {
        var result = await resultTask;
        return Check(result, check, error);
    }

    /// <summary>
    /// Evaluates a async condition on the value contained within the <see cref="Result{TValue}"/> and returns either the
    /// original result or an error.
    /// </summary>
    /// <remarks>This method allows chaining validation checks on a <see cref="Result{TValue}"/>. If the
    /// result does not contain a value or the condition defined by <paramref name="check"/> evaluates to <see
    /// langword="true"/>, the original result is returned. If the condition evaluates to <see langword="false"/>, the
    /// specified <paramref name="error"/> is returned.</remarks>
    /// <typeparam name="TValue">The type of the value contained within the <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result{TValue}"/> instance to check.</param>
    /// <param name="check">A function that defines the condition to evaluate on the value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the condition fails.</param>
    /// <returns>The original <see cref="Result{TValue}"/> if the condition is met or the result does not contain a value;
    /// otherwise, the specified <paramref name="error"/>.</returns>
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<bool>> check, Error error)
    {
        var result = await resultTask;
        return await CheckAsync(result, check, error);
    }
}
