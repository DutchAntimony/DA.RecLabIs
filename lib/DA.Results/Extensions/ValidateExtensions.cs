using DA.Results.Errors;

namespace DA.Results.Extensions;

/// <summary>
/// Validation: 
/// Validate the current result for a given predicate.
/// If the result is already a failure that is not a <see cref="ValidationError"/>, it will be returned as is.
/// If the result is either a success or a <see cref="ValidationError"/>, the predicate will be evaluated.
/// When the predicate returns true, the original result will be returned.
/// When the predicate returns false, a new <see cref="Result"/> will be returned that contains the original result's errors
/// as well as the provided <see cref="ValidationFailure"/>.
/// </summary>
public static class ValidateExtensions
{
    /// <summary>
    /// Validate the current result against a predicate.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="failure">The <see cref="ValidationFailure"/> to add if validation fails</param>
    /// <returns>The original <see cref="Result"/> if it is already a different failure or if the check function 
    /// does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result"/> representing the failure and any earlier failures.</returns>
    public static Result Validate(this Result result, Func<bool> predicate, ValidationFailure failure)
    {
        if (result.TryGetFailure(out var error) && error is not ValidationError)
        {
            return result; // If the result is already a failure different to a ValidationError, return it as is.
        }

        return predicate() ? result : MergeValidationErrors(result, failure);
    }

    /// <summary>
    /// Validate the current result against a predicate.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="failure">The <see cref="ValidationFailure"/> to add if validation fails</param>
    /// <returns>The original <see cref="Result"/> if it is already a different failure or if the check function 
    /// does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result"/> representing the failure and any earlier failures.</returns>
    public static async Task<Result> ValidateAsync(this Result result, Func<Task<bool>> predicate, ValidationFailure failure)
    {
        if (result.TryGetFailure(out var error) && error is not ValidationError)
        {
            return result; // If the result is already a failure different to a ValidationError, return it as is.
        }

        return await predicate() ? result : MergeValidationErrors(result, failure);
    }

    /// <summary>
    /// Validate the current result against a predicate.
    /// </summary>
    /// <param name="resultTask">The Task of <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="failure">The <see cref="ValidationFailure"/> to add if validation fails</param>
    /// <returns>The original <see cref="Result"/> if it is already a different failure or if the check function 
    /// does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result"/> representing the failure and any earlier failures.</returns>
    public static async Task<Result> Validate(this Task<Result> resultTask, Func<bool> predicate, ValidationFailure failure)
    {
        var result = await resultTask;
        return Validate(result, predicate, failure);
    }

    /// <summary>
    /// Validate the current result against a predicate.
    /// </summary>
    /// <param name="resultTask">The Task of <see cref="Result"/> instance to evaluate. Must not be null.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="failure">The <see cref="ValidationFailure"/> to add if validation fails</param>
    /// <returns>The original <see cref="Result"/> if it is already a different failure or if the check function 
    /// does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result"/> representing the failure and any earlier failures.</returns>
    public static async Task<Result> ValidateAsync(this Task<Result> resultTask, Func<Task<bool>> predicate, ValidationFailure failure)
    {
        var result = await resultTask;
        return await ValidateAsync(result, predicate, failure);
    }

    /// <summary>
    /// Validate the current result against a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TValue}"/> instance to evaluate. Must not be null.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="failure">The <see cref="ValidationFailure"/> to add if validation fails</param>
    /// <returns>The original <see cref="Result{TValue}"/> if it is already a different failure or if the check function 
    /// does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result{TValue}"/> representing the failure and any earlier failures.</returns>
    public static Result<TValue> Validate<TValue>(this Result<TValue> result, Func<bool> predicate, ValidationFailure failure)
    {
        if (result.TryGetFailure(out var error) && error is not ValidationError)
        {
            return result; // If the result is already a failure different to a ValidationError, return it as is.
        }

        return predicate() ? result : MergeValidationErrors(result, failure);
    }

    /// <summary>
    /// Validate the current result against a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="result">The <see cref="Result{TValue}"/> instance to evaluate. Must not be null.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="failure">The <see cref="ValidationFailure"/> to add if validation fails</param>
    /// <returns>The original <see cref="Result{TValue}"/> if it is already a different failure or if the check function 
    /// does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result{TValue}"/> representing the failure and any earlier failures.</returns>
    public static async Task<Result<TValue>> ValidateAsync<TValue>(this Result<TValue> result, Func<Task<bool>> predicate, ValidationFailure failure)
    {
        if (result.TryGetFailure(out var error) && error is not ValidationError)
        {
            return result; // If the result is already a failure different to a ValidationError, return it as is.
        }

        return await predicate() ? result : MergeValidationErrors(result, failure);
    }

    /// <summary>
    /// Validate the current result against a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result{TValue}"/> instance to evaluate. Must not be null.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="failure">The <see cref="ValidationFailure"/> to add if validation fails</param>
    /// <returns>The original <see cref="Result{TValue}"/> if it is already a different failure or if the check function 
    /// does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result{TValue}"/> representing the failure and any earlier failures.</returns>
    public static async Task<Result<TValue>> Validate<TValue>(this Task<Result<TValue>> resultTask, Func<bool> predicate, ValidationFailure failure)
    {
        var result = await resultTask;
        return Validate(result, predicate, failure);
    }

    /// <summary>
    /// Validate the current result against a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the input value contained in the original <see cref="Result{TValue}"/>.</typeparam>
    /// <param name="resultTask">The Task of <see cref="Result{TValue}"/> instance to evaluate. Must not be null.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="failure">The <see cref="ValidationFailure"/> to add if validation fails</param>
    /// <returns>The original <see cref="Result{TValue}"/> if it is already a different failure or if the check function 
    /// does not produce a failure. 
    /// Otherwise, returns a new <see cref="Result{TValue}"/> representing the failure and any earlier failures.</returns>
    public static async Task<Result<TValue>> ValidateAsync<TValue>(this Task<Result<TValue>> resultTask, Func<Task<bool>> predicate, ValidationFailure failure)
    {
        var result = await resultTask;
        return await ValidateAsync(result, predicate, failure);
    }

    /// <summary>
    /// Helper method to merge a validation failure into an existing result.
    /// </summary>
    private static Error MergeValidationErrors(IResult existing, ValidationFailure failure)
    {
        if (!existing.TryGetFailure(out var error))
        {
            return new ValidationError(failure);
        }

        return error switch
        {
            ValidationError validationError => new ValidationError(validationError.Failures.Add(failure)),
            _ => new UnexpectedError(new Exception($"Cannot merge validation error with existing error: {error}"))
        };
    }
}
