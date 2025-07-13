using DA.Guards;
using Shouldly;

namespace Guards.Tests.Unit;

public class StringGuardTests
{
    private const string _value = "test";

    [Fact]
    public void NotNullOrWhiteSpace_ReturnsValue_WhenStringIsNotNullOrEmpty()
    {
        var result = Guard.String.NotNullOrWhiteSpace(_value);
        result.ShouldBe(_value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\t")]
    [InlineData("\n   \t  \u2000")]
    public void NotNullOrEmpty_ThrowsArgumentException_WhenStringIsNullOrEmpty(string? value)
    {
        var exception = Should.Throw<ArgumentException>(() => Guard.String.NotNullOrWhiteSpace(value));
        exception.Message.ShouldContain(nameof(value));
        exception.Message.ShouldContain(nameof(NotNullOrEmpty_ThrowsArgumentException_WhenStringIsNullOrEmpty));
    }

    [Fact]
    public void MinimumLength_ReturnsValue_WhenStringMeetsMinimumLength()
    {
        var result = Guard.String.MinimumLength(_value, 4);
        result.ShouldBe(_value);
    }

    [Fact]
    public void MinimumLength_ThrowsArgumentException_WhenLengthIsTooShort()
    {
        var exception = Should.Throw<ArgumentException>(() => Guard.String.MinimumLength(_value, 5));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(MinimumLength_ThrowsArgumentException_WhenLengthIsTooShort));
    }

    [Fact]
    public void MaximumLength_ReturnsValue_WhenStringMeetsMaximumLength()
    {
        var result = Guard.String.MaximumLength(_value, 4);
        result.ShouldBe(_value);
    }

    [Fact]
    public void MaximumLength_ThrowsArgumentException_WhenLengthIsTooLong()
    {
        var exception = Should.Throw<ArgumentException>(() => Guard.String.MaximumLength(_value, 3));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(MaximumLength_ThrowsArgumentException_WhenLengthIsTooLong));
    }

    [Fact]
    public void ExactLength_ReturnsValue_WhenStringIsEqualLength()
    {
        var result = Guard.String.ExactLength(_value, 4);
        result.ShouldBe(_value);
    }

    [Fact]
    public void ExactLength_ThrowsArgumentException_WhenLengthIsTooShort()
    {
        var exception = Should.Throw<ArgumentException>(() => Guard.String.ExactLength(_value, 5));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(ExactLength_ThrowsArgumentException_WhenLengthIsTooShort));
    }

    [Fact]
    public void ExactLength_ThrowsArgumentException_WhenLengthIsTooLong()
    {
        var exception = Should.Throw<ArgumentException>(() => Guard.String.ExactLength(_value, 3));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(ExactLength_ThrowsArgumentException_WhenLengthIsTooLong));
    }

    [Fact]
    public void LengthBetween_ReturnsValue_WhenStringIsWithinRange()
    {
        var result = Guard.String.LengthBetween(_value, 3, 5);
        result.ShouldBe(_value);
    }

    [Fact]
    public void LengthBetween_ThrowsArgumentException_WhenLengthIsTooShort()
    {
        var exception = Should.Throw<ArgumentException>(() => Guard.String.LengthBetween(_value, 5, 6));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(LengthBetween_ThrowsArgumentException_WhenLengthIsTooShort));
    }

    [Fact]
    public void LengthBetween_ThrowsArgumentException_WhenLengthIsTooLong()
    {
        var exception = Should.Throw<ArgumentException>(() => Guard.String.LengthBetween(_value, 2, 3));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(LengthBetween_ThrowsArgumentException_WhenLengthIsTooLong));
    }

    [Fact]
    public void MatchesRegex_ReturnsValue_WhenRegexIsMatched()
    {
        var result = Guard.String.MatchesRegex(_value, "^[a-z]{4}$");
        result.ShouldBe(_value);
    }

    [Fact]
    public void MatchesRegex_ThrowsArgumentException_WhenRegexIsNotMatched()
    {
        var exception = Should.Throw<ArgumentException>(() => Guard.String.MatchesRegex(_value, "^[a-z]{5}$"));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(MatchesRegex_ThrowsArgumentException_WhenRegexIsNotMatched));
    }
}
