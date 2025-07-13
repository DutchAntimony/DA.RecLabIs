namespace Optional.Tests.Unit.Extensions;
public class ReduceTests : OptionTestBase
{
    private static string ThrowingString() => throw new ShouldAssertException("Should not be called");
    private static Task<string> ThrowingStringTask() => throw new ShouldAssertException("Should not be called");

    [Fact]
    public void Reduce_Should_RemainOriginal_IfOptionIsSome()
    {
        SomeOption.Reduce("def").ShouldBe(OptionValue);
        SomeOption.Reduce(ThrowingString).ShouldBe(OptionValue);
    }

    [Fact]
    public void Reduce_Should_ReplaceOriginal_IfOptionIsNone()
    {
        NoneOption.Reduce("def").ShouldBe("def");
        NoneOption.Reduce(() => "def").ShouldBe("def");
    }

    [Fact]
    public async Task Reduce_Should_BehaveCorrectlyWithAsyncOverloads()
    {
        (await SomeOptionTask.Reduce("def")).ShouldBe(OptionValue);
        (await SomeOptionTask.Reduce(ThrowingString)).ShouldBe(OptionValue);
        (await SomeOptionTask.ReduceAsync(ThrowingString)).ShouldBe(OptionValue);
        (await SomeOptionTask.ReduceAsync(ThrowingStringTask)).ShouldBe(OptionValue);
        (await SomeOption.ReduceAsync(ThrowingStringTask)).ShouldBe(OptionValue);
    }
}
