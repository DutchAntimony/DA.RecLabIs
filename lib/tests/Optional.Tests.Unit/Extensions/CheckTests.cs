namespace Optional.Tests.Unit.Extensions;

public class CheckTests : OptionTestBase
{
    private static bool PassingCheck(string value) => true;
    private static bool FailingCheck(string value) => false;
    private static Task<bool> FailingCheckTask(string value) => Task.FromResult(FailingCheck(value));
    private static bool ThrowingCheck(string value) => throw new ShouldAssertException("Should not be called");

    [Fact]
    public void Check_Should_ReturnOption_IfCheckPasses()
    {
        SomeOption.Check(PassingCheck).ShouldBe(SomeOption);
    }

    [Fact]
    public void Check_Should_ReturnNone_IfCheckFails()
    {
        SomeOption.Check(FailingCheck).ShouldBe(Option.None);
    }

    [Fact]
    public void Check_Should_ReturnNone_AndNotThrow_IfCheckFails()
    {
        NoneOption.Check(ThrowingCheck).ShouldBe(Option.None);
    }

    [Fact]
    public async Task Check_Should_BehaveCorrectlyWithAsyncOverloads()
    {
        (await SomeOption.CheckAsync(FailingCheckTask)).ShouldBe(Option.None);
        (await SomeOptionTask.Check(FailingCheck)).ShouldBe(Option.None);
        (await SomeOptionTask.CheckAsync(FailingCheckTask)).ShouldBe(Option.None);
    }
}