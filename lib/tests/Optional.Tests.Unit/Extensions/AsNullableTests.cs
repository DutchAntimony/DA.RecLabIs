namespace Optional.Tests.Unit.Extensions;
public class AsNullableTests : OptionTestBase
{
    [Fact]
    public void AsNullable_Should_ReturnNull_IfOptionIsNone()
    {
        NoneOption.AsNullable().ShouldBeNull();
    }

    [Fact]
    public void AsNullableValue_Should_ReturnNull_IfOptionIsNone()
    {
        Option<int> noneValue = Option.None;
        noneValue.AsNullableValue().ShouldBeNull();
    }

    [Fact]
    public void AsNullable_Should_ReturnValue_IfOptionIsSome()
    {
        SomeOption.AsNullable().ShouldBe(OptionValue);
    }

    [Fact]
    public void AsNullableValue_Should_ReturnValue_IfOptionIsSome()
    {
        Option<int> someValue = 123;
        someValue.AsNullableValue().ShouldBe(123);
    }

    [Fact]
    public async Task AsNullable_Should_BehaveCorrectlyWithAsyncOverloads()
    {
        (await SomeOptionTask.AsNullable()).ShouldBe(OptionValue);
        Task<Option<int>> valueTask = Task.FromResult(Option.Some(123));
        (await valueTask.AsNullableValue()).ShouldBe(123);
    }
}