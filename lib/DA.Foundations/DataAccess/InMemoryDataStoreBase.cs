using DA.Foundations.Entities;
using DA.Foundations.Examples;
using DA.Optional;
using DA.Results;
using DA.Results.Errors;
using System.Collections.Concurrent;

namespace DA.Foundations.DataAccess;

/// <summary>
/// Base class for all data stores that keep data in memory.
/// </summary>
public abstract class InMemoryDataStoreBase : IDataStore
{
    /// <summary>
    /// TypeParam object is a <c>ConcurrentDictionary{TKey, TEntity}</c>,
    /// <seealso cref="Load{TEntity, TKey}"/> method on where it get's filled.
    /// </summary>
    protected readonly ConcurrentDictionary<Type, object> _cache = new();

    /// <summary>
    /// The method of retreiving the data from the _cache may vary between implementations.
    /// </summary>
    /// <remarks>
    /// E.g. for a pure InMemoryDataStore, a new type can be added to the cache,
    /// whereas when the cache is an actual cache layer a new type may not be allowed.</remarks>
    protected abstract Task<ConcurrentDictionary<TKey, TEntity>> GetSet<TEntity, TKey>()
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey;

    /// <inheritdoc />
    public async Task<IQueryable<TEntity>> QueryAsync<TEntity, TKey>(CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        var set = await GetSet<TEntity, TKey>();
        return set.Values.AsQueryable();
    }

    /// <inheritdoc />
    public async Task<Option<TEntity>> FindByIdAsync<TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        var set = await GetSet<TEntity, TKey>();
        return set.TryGetValue(id, out var entity) ? entity : Option.None;
    }

    /// <inheritdoc />
    public async Task AddOrUpdateAsync<TEntity, TKey>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        var set = await GetSet<TEntity, TKey>();
        set[entity.Id] = entity;
    }

    /// <inheritdoc />
    public async Task<Result> RemoveAsync<TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        var set = await GetSet<TEntity, TKey>();
        return set.TryRemove(id, out _) ? Result.Success() : new NotFoundError(typeof(TEntity).Name, $"By Id '{id.Value}'");
    }

    /// <inheritdoc />
    public Task Clear<TEntity>(CancellationToken cancellationToken = default)
    {
        _cache.TryRemove(typeof(TEntity), out _); // If the set exists, remove it. If it doesn't, the set is already empty.
        return Task.CompletedTask;
    }

    /// <summary>
    /// Protected helper method to get all instances of a given TEntity in an IEnumerable.
    /// </summary>
    protected bool TryGetEntities<TEntity, TKey>(out IEnumerable<TEntity> entities)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        entities = [];
        if (!_cache.TryGetValue(typeof(TEntity), out var dictObj) ||
            dictObj is not ConcurrentDictionary<TKey, TEntity> dict)
        {
            return false;
        }

        entities = dict.Values;
        return true;
    }
}
