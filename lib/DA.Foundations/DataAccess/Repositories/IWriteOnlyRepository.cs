using DA.Foundations.Entities;
using DA.Results;

namespace DA.Foundations.DataAccess.Repositories;

/// <summary>
/// Generic repository for write only (POST, PUT, DELETE) operations.
/// </summary>
public interface IWriteOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : struct, IEntityKey
{
    /// <summary>
    /// Add a new entity to the collection.
    /// </summary>
    Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an existing entity in the collection.
    /// </summary>
    Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an entity from the collection.
    /// </summary>
    Task<Result> DeleteAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an entity from the collection.
    /// </summary>
    Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}