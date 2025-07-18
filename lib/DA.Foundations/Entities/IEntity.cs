namespace DA.Foundations.Entities;

/// <summary>
/// Contract for a domain entity.
/// When possible, prefer the use of <see cref="IEntity{TKey}"/> for a strongly typed Id.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Helper method to get the unique identifier of the entity.
    /// </summary>
    object GetId();
}

/// <summary>
/// Contract for a domain entity with a strongly typed identifier.
/// </summary>
/// <typeparam name="TKey">The type of the key, should be a <c>record struct</c> that inherits <see cref="IEntityKey"/></typeparam>
public interface IEntity<out TKey> : IEntity
    where TKey : struct, IEntityKey
{
    /// <summary>
    /// Unique strongly typed identifier for the entity.
    /// </summary>
    TKey Id { get; }
}