namespace Optional.Tests.Unit.Extensions;
public class BindTests : OptionTestBase
{
    private const string ExpectedFromOption = "abcdef";

    private static Option<string> Selector(string original) => original + "def";

    private static Task<Option<string>> SelectorTask(string original) => Task.FromResult(Selector(original));

    [Fact]
    public void Bind_Should_ReturnNone_IfOptionIsNone()
    {
        NoneOption.Bind(Selector).ShouldBe(Option.None);
    }

    [Fact]
    public void Bind_Should_ReturnSome_IfOptionIsSome()
    {
        SomeOption.Bind(Selector).ShouldBe(ExpectedFromOption);
    }

    [Fact]
    public async Task Bind_Should_BehaveCorrectlyWithAsyncOverloads()
    {
        (await SomeOption.BindAsync(SelectorTask)).ShouldBe(ExpectedFromOption);
        (await SomeOptionTask.Bind(Selector)).ShouldBe(ExpectedFromOption);
        (await SomeOptionTask.BindAsync(SelectorTask)).ShouldBe(ExpectedFromOption);
    }
}
