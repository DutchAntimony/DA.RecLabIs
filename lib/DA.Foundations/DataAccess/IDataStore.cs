using DA.Foundations.Entities;
using DA.Optional;
using DA.Results;

namespace DA.Foundations.DataAccess;

/// <summary>
/// Data provider interface for generic data access operations.
/// Has options similar to EF Core DbContext, but is not tied to any specific ORM.
/// </summary>
public interface IDataStore
{
    /// <summary>
    /// Get all entities of a specified type in the data store.
    /// </summary>
    /// <returns></returns>
    Task<IQueryable<TEntity>> QueryAsync<TEntity, TKey>(CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey;

    /// <summary>
    /// Find an entity of a specified type in the data store that by its id.
    /// </summary>
    /// <param name="id">The id of the entity to find.</param>
    /// <returns>An <see cref="Option{TEntity}"/> that either contains the entity or if not found an <see cref="Option.None"/></returns>
    Task<Option<TEntity>> FindByIdAsync<TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey;

    /// <summary>
    /// Add a new or update an existing entity.
    /// </summary>
    /// <param name="entity">The current state of the entity.</param>
    Task AddOrUpdateAsync<TEntity, TKey>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey;

    /// <summary>
    /// Remove an entity of a specified type from the data store by its id.
    /// </summary>
    /// <param name="id">The id of the entity to remove.</param>
    /// <returns>A <see cref="Result"/> indicating if the entity could be removed.</returns>
    Task<Result> RemoveAsync<TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey;

    /// <summary>
    /// Clear all entities of a specified type in the data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to clear</typeparam>
    Task Clear<TEntity>(CancellationToken cancellationToken = default);
}
