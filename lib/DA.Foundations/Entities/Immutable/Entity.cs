namespace DA.Foundations.Entities.Immutable;

/// <summary>
/// Base record for an immutable entity with a unique strongly typed identifier.
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract record Entity<TKey> : IEntity<TKey>
    where TKey : struct, IEntityKey
{
    /// <inheritdoc />
    public TKey Id { get; protected init; }

    /// <summary>
    /// Constructor for a new entity with a generated ID.
    /// </summary>
    protected Entity()
    {
        Id = new() { Value = Guid.CreateVersion7() };
    }

    /// <summary>
    /// Constructor for an existing entity with a specified ID.
    /// </summary>
    /// <param name="id">The id of the existing entity.</param>
    protected Entity(TKey id)
    {
        Id = id;
    }

    /// <inheritdoc />
    public object GetId() => Id;
}