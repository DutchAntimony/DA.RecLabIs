using System.Collections.Concurrent;

namespace DA.Foundations.DataAccess;

/// <summary>
/// Data provider implementation that stores data in memory.
/// </summary>
public sealed class InMemoryDataStore : InMemoryDataStoreBase
{
    protected override async Task<ConcurrentDictionary<TKey, TEntity>> GetSet<TEntity, TKey>()
    {
        await Task.Yield();
        var type = typeof(TEntity);
        return (ConcurrentDictionary<TKey, TEntity>)_cache.GetOrAdd(type, _ => new ConcurrentDictionary<TKey, TEntity>());
    }
}
