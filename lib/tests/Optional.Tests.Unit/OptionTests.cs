namespace Optional.Tests.Unit;

public class OptionTests : OptionTestBase
{
    [Fact]
    public void ToString_Should_ReturnValue_ForSomeOption()
    {
        SomeOption.ToString().ShouldBe(OptionValue);
    }

    [Fact]
    public void ToString_Should_ReturnEmptyString_ForNoneOption()
    {
        NoneOption.ToString().ShouldBe(string.Empty);
    }

    [Fact]
    public void Equals_ShouldBeTrue_ForEqualOption()
    {
        Option<string> shadow = "abc";

        (SomeOption == "abc").ShouldBeTrue();
        (SomeOption == "def").ShouldBeFalse();
        (NoneOption == string.Empty).ShouldBeFalse();
        (SomeOption != "def").ShouldBeTrue();

        (SomeOption == shadow).ShouldBeTrue();
        (SomeOption == Option.Some("abcd")).ShouldBeFalse();
        (NoneOption == shadow).ShouldBeFalse();
        (SomeOption != shadow).ShouldBeFalse();

        SomeOption.Equals(null).ShouldBeFalse();
        SomeOption.Equals("abc").ShouldBeTrue();
        SomeOption.Equals(shadow).ShouldBeTrue();

        SomeOption.Equals((object?)null).ShouldBeFalse();
        SomeOption.Equals((object?)"abc").ShouldBeTrue();
        SomeOption.Equals(4).ShouldBeFalse();
        SomeOption.Equals((object?)shadow).ShouldBeTrue();

        NoneOption.Equals(null).ShouldBeTrue();
        NoneOption.Equals("def").ShouldBeFalse();
        NoneOption.Equals(Option.None).ShouldBeTrue();
    }

    [Fact]
    public void GetHashCode_Should_ReturnHashcodeOfObject_IfOptionIsSome()
    {
        SomeOption.GetHashCode().ShouldBe("abc".GetHashCode());
    }

    [Fact]
    public void GetHashCode_Should_ReturnZero_IfOptionIsNone()
    {
        NoneOption.GetHashCode().ShouldBe(0);
    }
}