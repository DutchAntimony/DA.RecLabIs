namespace Optional.Tests.Unit.Extensions;

public class MatchTests : OptionTestBase
{
    private static int WhenOptionIsSome(string original) => 1;
    private static int WhenOptionIsNone() => 0;
        private static Task<int> WhenOptionIsSomeTask(string original) => Task.FromResult(WhenOptionIsSome(original));
    private static Task<int> ThrowTask() => throw new ShouldAssertException("Should not be called");

    [Fact]
    public void Match_Should_Return0_IfOptionIsNone()
    {
        NoneOption.Match(WhenOptionIsSome, WhenOptionIsNone).ShouldBe(0);
    }

    [Fact]
    public void Match_Should_Return1_IfOptionIsSome()
    {
        SomeOption.Match(WhenOptionIsSome, WhenOptionIsNone).ShouldBe(1);
    }

    [Fact]
    public async Task Match_Should_BehaveCorrectlyWithAsyncOverloads()
    {
        (await SomeOption.MatchAsync(WhenOptionIsSomeTask, ThrowTask)).ShouldBe(1);
        (await SomeOptionTask.Match(WhenOptionIsSome, WhenOptionIsNone)).ShouldBe(1);
        (await SomeOptionTask.MatchAsync(WhenOptionIsSomeTask, ThrowTask)).ShouldBe(1);
    }
}
