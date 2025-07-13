namespace Optional.Tests.Unit.Extensions;

public class MapToOptionTests : OptionTestBase
{
    private const string ExpectedFromOption = "abcdef";
    private static string? Selector(string original) => original + "def";
    private static Task<string?> SelectorTask(string original) => Task.FromResult(Selector(original));

    [Fact]
    public void Map_Should_ReturnNone_IfOptionIsNone()
    {
        NoneOption.Map(Selector).ShouldBe(Option.None);
    }

    [Fact]
    public void Map_Should_ReturnSome_IfOptionIsSome()
    {
        SomeOption.Map(Selector).ShouldBe(ExpectedFromOption);
    }


    [Fact]
    public async Task Map_Should_BehaveCorrectlyWithAsyncOverloads()
    {
        (await SomeOption.MapAsync(SelectorTask)).ShouldBe(ExpectedFromOption);
        (await SomeOptionTask.Map(Selector)).ShouldBe(ExpectedFromOption);
        (await SomeOptionTask.MapAsync(SelectorTask)).ShouldBe(ExpectedFromOption);
    }
}
