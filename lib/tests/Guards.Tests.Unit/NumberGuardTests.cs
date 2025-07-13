using DA.Guards;
using Shouldly;

namespace Guards.Tests.Unit;
public class NumberGuardTests
{
    private int _value = 10;
    private int _lower = 9;
    private int _higher = 11;

    [Fact]
    public void GreaterThan_ReturnsValue_WhenMoreThanComparisonValue()
    {
        var result = Guard.Number.GreaterThan(_value, _lower);
        result.ShouldBe(_value);
    }

    [Fact]
    public void GreaterThan_ThrowsArgumentOutOfRangeException_WhenEqualToComparisonValue()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.GreaterThan(_value, _value));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(GreaterThan_ThrowsArgumentOutOfRangeException_WhenEqualToComparisonValue));
    }

    [Fact]
    public void GreaterThan_ThrowsArgumentOutOfRangeException_WhenLessThanComparisonValue()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.GreaterThan(_value, _higher));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(GreaterThan_ThrowsArgumentOutOfRangeException_WhenLessThanComparisonValue));
    }

    [Fact]
    public void GreaterThanOrEqualTo_ReturnsValue_WhenMoreThanComparisonValue()
    {
        var result = Guard.Number.GreaterThanOrEqualTo(_value, _lower);
        result.ShouldBe(_value);
    }

    [Fact]
    public void GreaterThanOrEqualTo_ReturnsValue_WhenEqualComparisonValue()
    {
        var result = Guard.Number.GreaterThanOrEqualTo(_value, _value);
        result.ShouldBe(_value);
    }

    [Fact]
    public void GreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_WhenLessThanComparisonValue()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.GreaterThanOrEqualTo(_value, _higher));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(GreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_WhenLessThanComparisonValue));
    }

    [Fact]
    public void SmallerThan_ReturnsValue_WhenLessThanComparisonValue()
    {
        var result = Guard.Number.SmallerThan(_value, _higher);
        result.ShouldBe(_value);
    }

    [Fact]
    public void SmallerThan_ThrowsArgumentOutOfRangeException_WhenEqualToComparisonValue()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.SmallerThan(_value, _value));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(SmallerThan_ThrowsArgumentOutOfRangeException_WhenEqualToComparisonValue));
    }

    [Fact]
    public void SmallerThan_ThrowsArgumentOutOfRangeException_WhenMoreThanComparisonValue()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.SmallerThan(_value, _lower));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(SmallerThan_ThrowsArgumentOutOfRangeException_WhenMoreThanComparisonValue));
    }

    [Fact]
    public void SmallerThanOrEqualTo_ReturnsValue_WhenLessThanComparisonValue()
    {
        var result = Guard.Number.SmallerThanOrEqualTo(_value, _higher);
        result.ShouldBe(_value);
    }

    [Fact]
    public void SmallerThanOrEqualTo_ReturnsValue_WhenEqualComparisonValue()
    {
        var result = Guard.Number.SmallerThanOrEqualTo(_value, _value);
        result.ShouldBe(_value);
    }

    [Fact]
    public void SmallerThanOrEqualTo_ThrowsArgumentOutOfRangeException_WhenMoreThanComparisonValue()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.SmallerThanOrEqualTo(_value, _lower));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(SmallerThanOrEqualTo_ThrowsArgumentOutOfRangeException_WhenMoreThanComparisonValue));
    }

    [Fact]
    public void IsBetween_ReturnsValue_WhenInRange()
    {
        var result = Guard.Number.IsBetween(_value, _lower, _higher);
        result.ShouldBe(_value);
    }

    [Fact]
    public void IsBetween_ThrowsArgumentOutOfRangeException_WhenLowerThanRange()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.IsBetween(_value, _higher, _higher + 1));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(IsBetween_ThrowsArgumentOutOfRangeException_WhenLowerThanRange));
    }

    [Fact]
    public void IsBetween_ThrowsArgumentOutOfRangeException_WhenHigherThanRange()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.IsBetween(_value, _lower - 1, _lower));
        exception.Message.ShouldContain(nameof(_value));
        exception.Message.ShouldContain(nameof(IsBetween_ThrowsArgumentOutOfRangeException_WhenHigherThanRange));
    }

    [Fact]
    public void Positive_ReturnsValue_WhenPositive()
    {
        var result = Guard.Number.Positive(_value);
        result.ShouldBe(_value);
    }

    [Fact]
    public void Positive_ThrowsArgumentOutOfRangeException_WhenZero()
    {
        var value = 0;
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.Positive(value));
        exception.Message.ShouldContain(nameof(value));
        exception.Message.ShouldContain(nameof(Positive_ThrowsArgumentOutOfRangeException_WhenZero));
    }

    [Fact]
    public void Positive_ThrowsArgumentOutOfRangeException_WhenNegative()
    {
        var value = -1;
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.Positive(value));
        exception.Message.ShouldContain(nameof(value));
        exception.Message.ShouldContain(nameof(Positive_ThrowsArgumentOutOfRangeException_WhenNegative));
    }

    [Fact]
    public void NotNegative_ReturnsValue_WhenPositive()
    {
        var result = Guard.Number.NotNegative(_value);
        result.ShouldBe(_value);
    }

    [Fact]
    public void NotNegative_ReturnsValue_WhenZero()
    {
        var value = 0;
        var result = Guard.Number.NotNegative(value);
        result.ShouldBe(value);
    }

    [Fact]
    public void NotNegative_ThrowsArgumentOutOfRangeException_WhenNegative()
    {
        var value = -1;
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Number.NotNegative(value));
        exception.Message.ShouldContain(nameof(value));
        exception.Message.ShouldContain(nameof(NotNegative_ThrowsArgumentOutOfRangeException_WhenNegative));
    }
}
