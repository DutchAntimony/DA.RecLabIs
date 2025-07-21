using Foundations.Tests.Unit.Samples;

namespace Foundations.Tests.Unit.Immutable;
public class EntityTests
{
    [Fact]
    public void ImmutableEntity_ShouldExpose_GetIdMethod()
    {
        var id = new ImmutableTestEntityId(Guid.NewGuid());
        var testEntity = new ImmutableTestEntityType(id, 1);

        testEntity.Number.ShouldBe(1);

        testEntity.GetId().ToString().ShouldBe(id.ToString());
    }
}
