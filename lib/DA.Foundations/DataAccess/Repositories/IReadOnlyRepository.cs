using DA.Foundations.Entities;
using DA.Optional;
using DA.Results;
using DA.Results.Errors;
using System.Linq.Expressions;

namespace DA.Foundations.DataAccess.Repositories;

/// <summary>
/// Generic repository for read only (GET) operations.
/// </summary>
public interface IReadOnlyRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : struct, IEntityKey
{
    /// <summary>
    /// Get all entities in the collection.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all entities in the collection that satisfy a predicate.
    /// </summary>
    /// <param name="predicate">The condition the entities must satisfy.</param>
    Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the entity with the specified id.
    /// </summary>
    /// <param name="id">The id to check for.</param>
    /// <returns>Success result with the entity with that Id if found, and a Result with a NotFound error if not.</returns>
    Task<Result<TEntity>> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an entity with the specified id exists in the collection.
    /// </summary>
    /// <param name="id">The id to check for.</param>
    Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Count the number of entities in the collection.
    /// </summary>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Count the number of entities in the collection that satisfy a predicate.
    /// </summary>
    /// <param name="predicate">The condition the entities must satisfy.</param>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single entity that satisfies a predicate.
    /// </summary>
    /// <param name="predicate">The condition the entities must satisfy.</param>
    /// <returns>Success <see cref="Result{TEntity}"> with the entity with that predicate if found,
    /// a Failure Result with a <see cref="NotFoundError"> if none is found,
    /// and a Failure Result with a <see cref="UnexpectedError"/> if more than one entity is found.</returns>
    Task<Result<TEntity>> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single entity that satisfies a predicate.
    /// </summary>
    /// <param name="predicate">The condition the entities must satisfy.</param>
    /// <returns>An <see cref="Option{TEntity}"/> containing the entity with that predicate if found,
    /// and a <see cref="Option.None"/> if none is found.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown if the collection contains more than one element.</exception>
    Task<Option<TEntity>> GetSingleOrNoneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
