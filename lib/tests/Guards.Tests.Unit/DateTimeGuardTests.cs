using DA.Guards;
using Shouldly;

namespace Guards.Tests.Unit;

public class DateTimeGuardTests
{
    private readonly DateTime _date = new(2023, 10, 1);
    private readonly DateTime _earlier = new(2023, 9, 30);
    private readonly DateTime _later = new(2023, 10, 2);

    private readonly DateOnly _earlierDateOnly = new(2023, 9, 30);
    private readonly DateOnly _laterDateOnly = new(2023, 10, 2);

    [Fact]
    public void After_EarlierDate_ReturnsValue()
    {
        var result = Guard.Date.After(_date, _earlier);
        result.ShouldBe(_date);
    }

    [Fact]
    public void After_LaterDate_Throws()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Date.After(_date, _later));
        exception.Message.ShouldContain(nameof(_date));
        exception.Message.ShouldContain(nameof(After_LaterDate_Throws));
    }

    [Fact]
    public void Before_LaterDate_ReturnsValue()
    {
        var result = Guard.Date.Before(_date, _later);
        result.ShouldBe(_date);
    }

    [Fact]
    public void Before_EarlierDate_Throws()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Date.Before(_date, _earlier));
        exception.Message.ShouldContain(nameof(_date));
        exception.Message.ShouldContain(nameof(Before_EarlierDate_Throws));
    }

    [Fact]
    public void InRange_OfCorrectRange_ReturnsValue()
    {
        var result = Guard.Date.InRange(_date, _earlier, _later);
        result.ShouldBe(_date);
    }

    [Fact]
    public void InRange_EarlierDates_Throws()
    {
        var evenEarlier = new DateTime(2023, 9, 29);
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Date.InRange(_date, evenEarlier, _earlier));
        exception.Message.ShouldContain(nameof(_date));
        exception.Message.ShouldContain(nameof(InRange_EarlierDates_Throws));
    }

    [Fact]
    public void InRange_LaterDates_Throws()
    {
        var evenLater = new DateTime(2023, 10, 3);
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Date.InRange(_date, _later, evenLater));
        exception.Message.ShouldContain(nameof(_date));
        exception.Message.ShouldContain(nameof(InRange_LaterDates_Throws));
    }

    [Fact]
    public void After_EarlierDateOnly_ReturnsValue()
    {
        var result = Guard.Date.After(_date, _earlierDateOnly);
        result.ShouldBe(_date);
    }

    [Fact]
    public void After_LaterDateOnly_Throws()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Date.After(_date, _laterDateOnly));
        exception.Message.ShouldContain(nameof(_date));
        exception.Message.ShouldContain(nameof(After_LaterDateOnly_Throws));
    }

    [Fact]
    public void Before_LaterDateOnly_ReturnsValue()
    {
        var result = Guard.Date.Before(_date, _laterDateOnly);
        result.ShouldBe(_date);
    }

    [Fact]
    public void Before_EarlierDateOnly_Throws()
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Date.Before(_date, _earlierDateOnly));
        exception.Message.ShouldContain(nameof(_date));
        exception.Message.ShouldContain(nameof(Before_EarlierDateOnly_Throws));
    }

    [Fact]
    public void InRange_OfCorrectDateOnlyRange_ReturnsValue()
    {
        var result = Guard.Date.InRange(_date, _earlierDateOnly, _laterDateOnly);
        result.ShouldBe(_date);
    }

    [Fact]
    public void InRange_EarlierDateOnlyDates_Throws()
    {
        var evenEarlier = new DateOnly(2023, 9, 29);
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Date.InRange(_date, evenEarlier, _earlierDateOnly));
        exception.Message.ShouldContain(nameof(_date));
        exception.Message.ShouldContain(nameof(InRange_EarlierDateOnlyDates_Throws));
    }

    [Fact]
    public void InRange_LaterDateOnlyDates_Throws()
    {
        var evenLater = new DateOnly(2023, 10, 3);
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => Guard.Date.InRange(_date, _laterDateOnly, evenLater));
        exception.Message.ShouldContain(nameof(_date));
        exception.Message.ShouldContain(nameof(InRange_LaterDateOnlyDates_Throws));
    }
}