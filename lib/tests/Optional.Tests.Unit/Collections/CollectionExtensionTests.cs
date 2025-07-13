using DA.Optional.Collections;

namespace Optional.Tests.Unit.Collections;

public class CollectionExtensionTests
{
    private readonly IEnumerable<Option<string>> _optionCollection = [Option.None, "A", "AB", Option.None, "C", Option.None];
    private readonly IEnumerable<Option<string>> _emptyCollection = [Option.None, Option.None];
    private readonly IEnumerable<string> _stringCollection = ["A", "AB", "C"];
    private readonly IEnumerable<string> _emptyStringCollection = [];

    [Fact]
    public void Values_Should_ReturnAllNoneValues_FromCollection()
    {
        _optionCollection.Values().Count().ShouldBe(3);
        _emptyCollection.Values().Count().ShouldBe(0);
    }

    [Fact]
    public void Values_Should_ReturnAllNoneValuesMatchingCriteria_FromCollection()
    {
        _optionCollection.Values(v => v.StartsWith('A')).Count().ShouldBe(2);
        _optionCollection.Values(v => v.StartsWith('Z')).Count().ShouldBe(0);
        _emptyCollection.Values(v => v.StartsWith('A')).Count().ShouldBe(0);
    }

    [Fact]
    public void FirstOrNone_Should_ReturnFirstValue_FromOptionCollection()
    {
        _optionCollection.FirstOrNone().ShouldBe("A");
        _emptyCollection.FirstOrNone().ShouldBe(Option.None);
    }

    [Fact]
    public void FirstOrNone_Should_ReturnFirstValueMatchingCriteria_FromOptionCollection()
    {
        _optionCollection.FirstOrNone(v => v.StartsWith('A')).ShouldBe("A");
        _optionCollection.FirstOrNone(v => v.StartsWith('Z')).ShouldBe(Option.None);
        _emptyCollection.FirstOrNone(v => v.StartsWith('A')).ShouldBe(Option.None);
    }

    [Fact]
    public void FirstOrNone_Should_ReturnFirstValue_FromStringCollection()
    {
        _stringCollection.FirstOrNone().ShouldBe("A");
        _emptyStringCollection.FirstOrNone().ShouldBe(Option.None);
    }

    [Fact]
    public void FirstOrNone_Should_ReturnFirstValueMatchingCriteria_FromStringCollection()
    {
        _stringCollection.FirstOrNone(v => v.StartsWith("C")).ShouldBe("C");
        _stringCollection.FirstOrNone(v => v.StartsWith("X")).ShouldBe(Option.None);
        _emptyStringCollection.FirstOrNone(v => true).ShouldBe(Option.None);
    }

    [Fact]
    public void CountValues_Should_ReturnTheAmountOfNotNoneValues_FromCollection()
    {
        _optionCollection.CountValues().ShouldBe(3);
        _emptyCollection.CountValues().ShouldBe(0);
    }

    [Fact]
    public void CountValues_Should_ReturnTheAmountOfNotNoneValues_MatchingCriteria_FromCollection()
    {
        _optionCollection.CountValues(v => v.StartsWith('A')).ShouldBe(2);
        _optionCollection.CountValues(v => v.StartsWith('Z')).ShouldBe(0);
        _emptyCollection.CountValues(v => v.StartsWith('A')).ShouldBe(0);
    }

    [Fact]
    public void AnyValues_Should_ReturnTrue_IfAnyValuesExist_FromCollection()
    {
        _optionCollection.AnyValues().ShouldBeTrue();
        _emptyCollection.AnyValues().ShouldBeFalse();
    }

    [Fact]
    public void AnyValues_Should_ReturnTrue_IfAnyValuesMatched_FromCollection()
    {
        _optionCollection.AnyValues(v => v.StartsWith('A')).ShouldBeTrue();
        _optionCollection.AnyValues(v => v.StartsWith('Z')).ShouldBeFalse();
        _emptyCollection.AnyValues(v => v.StartsWith('A')).ShouldBeFalse();
    }
}

