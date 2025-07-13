using DA.Guards;
using Shouldly;

namespace Guards.Tests.Unit;
public class GuidGuardTests
{
    [Fact]
    public void NotEmpty_ReturnsValue_WhenGuidIsNotEmpty()
    {
        var guid = Guid.NewGuid();
        var result = Guard.Guid.NotEmpty(guid);
        result.ShouldBe(guid);
    }

    [Fact]
    public void NotEmpty_ThrowsArgumentException_WhenGuidIsEmpty()
    {
        var guid = Guid.Empty;
        var exception = Should.Throw<ArgumentException>(() => Guard.Guid.NotEmpty(guid));
        exception.Message.ShouldContain(nameof(guid));
        exception.Message.ShouldContain(nameof(NotEmpty_ThrowsArgumentException_WhenGuidIsEmpty));
    }

    [Fact]
    public void Version7_ReturnsValue_WhenGuidIsVersion7()
    {
        var guid = Guid.CreateVersion7();
        var result = Guard.Guid.Version7(guid);
        result.ShouldBe(guid);
    }

    [Fact]
    public void Version7_ThrowsArgumentException_WhenGuidIsNotVersion7()
    {
        var guid = Guid.NewGuid();
        var exception = Should.Throw<ArgumentException>(() => Guard.Guid.Version7(guid));
        exception.Message.ShouldContain(nameof(guid));
        exception.Message.ShouldContain(nameof(Version7_ThrowsArgumentException_WhenGuidIsNotVersion7));
    }
}
