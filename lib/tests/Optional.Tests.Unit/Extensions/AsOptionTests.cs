namespace Optional.Tests.Unit.Extensions;

public class AsOptionTests : OptionTestBase
{
    [Fact]
    public async Task AsOption_Should_ReturnNoneOption_IfValueIsNull()
    {
        var nullTask = Task.FromResult<string?>(null);
        (await nullTask.AsOption()).TryGetValue(out _).ShouldBeFalse();
    }

    [Fact]
    public async Task AsOption_Should_ReturnSomeOption_IfValueIsNotNull()
    {
        var nullTask = Task.FromResult<string?>(OptionValue);
        (await nullTask.AsOption()).TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldBe(OptionValue);
    }

    [Fact]
    public async Task AsOption_Should_ReturnNoneValueOption_IfValueIsNull()
    {
        var nullTask = Task.FromResult<int?>(null);
        (await nullTask.AsOption()).TryGetValue(out _).ShouldBeFalse();
    }

    [Fact]
    public async Task AsOption_Should_ReturnSomeValueOption_IfValueIsNotNull()
    {
        var nullTask = Task.FromResult<int?>(123);
        (await nullTask.AsOption()).TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldBe(123);
    }
}
