namespace Foundations.Tests.Unit.Helpers;

internal static class ResultTestExtensions
{
    public static void ShouldBeSuccessWithValue<TValue>(this Result<TValue> result, TValue expected)
    {
        result.ShouldBeOfType<Result<TValue>>();
        result.TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldBe(expected);
    }

    public static void ShouldBeFailureWithError(this IResult result, Error expectedError)
    {
        result.TryGetFailure(out var actualError).ShouldBeTrue();
        actualError.ShouldBe(expectedError);
    }

    public static void ShouldBeFailureWithError<TValue>(this Result<TValue> result, Error expectedError)
    {
        result.ShouldBeOfType<Result<TValue>>();
        result.TryGetFailure(out var actualError).ShouldBeTrue();
        actualError.ShouldBe(expectedError);
    }

    public static void ShouldBeFailureOfType<TExpectedError>(this IResult result, string? expectedMessageContains = null)
        where TExpectedError : Error
    {
        result.TryGetFailure(out var actualError).ShouldBeTrue();
        actualError.ShouldBeOfType<TExpectedError>();
        actualError.Message.ShouldContain(expectedMessageContains ?? string.Empty);
    }
}