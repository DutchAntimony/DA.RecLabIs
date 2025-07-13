using DA.Guards;
using Shouldly;

namespace Guards.Tests.Unit;

public class ThatGuardTests
{
    [Fact]
    public void NotNull_ReturnsValue_WhenValueIsNotNull_ForClasses()
    {
        var value = new object();
        var result = Guard.That.NotNull(value);
        result.ShouldBe(value);
    }

    [Fact]
    public void NotNull_ThrowsArgumentNullException_WhenValueIsNull_ForClasses()
    {
        object? value = null;
        var exception = Should.Throw<ArgumentNullException>(() => Guard.That.NotNull(value));
        exception.Message.ShouldContain(nameof(value));
        exception.Message.ShouldContain(nameof(NotNull_ThrowsArgumentNullException_WhenValueIsNull_ForClasses));
    }

    [Fact]
    public void NotNull_ReturnsValue_WhenValueIsNotNull_ForStructs()
    {
        int? value = new int();
        var result = Guard.That.NotNull(value);
        result.ShouldBe(value.Value);
    }

    [Fact]
    public void NotNull_ThrowsArgumentNullException_WhenValueIsNull_ForStructs()
    {
        int? value = null;
        var exception = Should.Throw<ArgumentNullException>(() => Guard.That.NotNull(value));
        exception.Message.ShouldContain(nameof(value));
        exception.Message.ShouldContain(nameof(NotNull_ThrowsArgumentNullException_WhenValueIsNull_ForStructs));
    }

    [Fact]
    public void NotDefault_ReturnsValue_WhenValueIsNotDefault()
    {
        int value = 5;
        var result = Guard.That.NotDefault(value);
        result.ShouldBe(value);
    }

    [Fact]
    public void NotDefault_ThrowsArgumentNullException_WhenValueIsDefault()
    {
        int value = default;
        var exception = Should.Throw<ArgumentNullException>(() => Guard.That.NotDefault(value));
        exception.Message.ShouldContain(nameof(value));
        exception.Message.ShouldContain(nameof(NotDefault_ThrowsArgumentNullException_WhenValueIsDefault));
    }

    [Fact]
    public void True_ReturnsValue_WhenPredicateIsTrue()
    {
        var value = 5;
        var result = Guard.That.True(value, v => v > 0);
        result.ShouldBe(value);
    }

    [Fact]
    public void True_ThrowsArgumentException_WhenPredicateIsFalse()
    {
        var value = 5;
        var exception = Should.Throw<ArgumentException>(() => Guard.That.True(value, v => v < 0));
        exception.Message.ShouldContain(nameof(value));
        exception.Message.ShouldContain(nameof(True_ThrowsArgumentException_WhenPredicateIsFalse));
    }

    [Fact]
    public async Task TrueAsync_ReturnsValue_WhenPredicateIsTrueAsync()
    {
        var value = 5;
        var result = await Guard.That.TrueAsync(value, async v => await Task.FromResult(v > 0));
        result.ShouldBe(value);
    }

    [Fact]
    public async Task TrueAsync_ThrowsArgumentException_WhenPredicateIsFalseAsync()
    {
        var value = 5;
        var exception = await Should.ThrowAsync<ArgumentException>(() => Guard.That.TrueAsync(value, 
            async v => await Task.FromResult(v < 0)));
        exception.Message.ShouldContain(nameof(value));
        exception.Message.ShouldContain(nameof(TrueAsync_ThrowsArgumentException_WhenPredicateIsFalseAsync));
    }
}
