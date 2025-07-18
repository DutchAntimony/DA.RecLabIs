using DA.Foundations.Entities;
using DA.Optional;
using DA.Results;
using DA.Results.Errors;
using System.Collections.Concurrent;

namespace DA.Foundations.DataAccess;

/// <summary>
/// Data provider implementation that stores data in memory.
/// </summary>
public sealed class InMemoryDataStore : IDataStore
{
    private readonly ConcurrentDictionary<Type, object> _sets = new();

    private ConcurrentDictionary<TKey, TEntity> GetOrCreateSet<TEntity, TKey>()
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        var type = typeof(TEntity);
        return (ConcurrentDictionary<TKey, TEntity>)_sets.GetOrAdd(type, _ => new ConcurrentDictionary<TKey, TEntity>());
    }

    public Task<IQueryable<TEntity>> QueryAsync<TEntity, TKey>(CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey => Task.FromResult(GetOrCreateSet<TEntity, TKey>().Values.AsQueryable());

    public Task<Option<TEntity>> FindByIdAsync<TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        var set = GetOrCreateSet<TEntity, TKey>();
        Option<TEntity> result = set.TryGetValue(id, out var entity) ? entity : Option.None;
        return Task.FromResult(result);
    }

    public Task AddOrUpdateAsync<TEntity, TKey>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        var set = GetOrCreateSet<TEntity, TKey>();
        set[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task<Result> RemoveAsync<TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        var set = GetOrCreateSet<TEntity, TKey>();
        var result = set.TryRemove(id, out _) ? Result.Success() : new NotFoundError(typeof(TEntity).Name, $"By Id '{id.Value}'");
        return Task.FromResult(result);
    }

    public Task Clear<TEntity>(CancellationToken cancellationToken = default)
    {
        var type = typeof(TEntity);
        _sets.TryRemove(type, out _); // If the set exists, remove it. If it doesn't, the set is already empty.
        return Task.CompletedTask;
    }
}
