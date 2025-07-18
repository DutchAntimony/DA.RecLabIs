using DA.Foundations.Entities;
using DA.Foundations.Entities.Immutable;

namespace Foundations.Tests.Unit.Samples;

internal readonly record struct ImmutableTestEntityId(Guid Value) : IEntityKey;
internal record ImmutableTestEntityType(ImmutableTestEntityId Id, int Number) : Entity<ImmutableTestEntityId>(Id);