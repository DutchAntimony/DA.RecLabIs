using DA.Foundations.DataAccess.Persistence;
using DA.Optional;
using DA.Results;
using DA.Results.Extensions;
using System.Collections.Concurrent;

namespace DA.Foundations.DataAccess.Sessions;

/// <summary>
/// A SessionDataStore is a <see cref="IFileDataStore"/> that works with a <typeparamref name="TSession"/> to map the data, and a
/// <paramref name="persistenceStrategy"/> to interact with the mapped data to and from the disk.
/// </summary>
/// <typeparam name="TSession">The session that contains the mapping logic to and from a snapshot that can be stored by the <paramref name="persistenceStrategy"/></typeparam>
/// <typeparam name="TSessionSnapshot">The serializable variant of the <typeparamref name="TSession"/>.</typeparam>
/// <param name="persistenceStrategy">The method to store the <typeparamref name="TSessionSnapshot"/> to the disk.</param>
internal sealed class SessionDataStore<TSession, TSessionSnapshot>
    (ISnapshotPersistenceStrategy<TSessionSnapshot> persistenceStrategy): InMemoryDataStoreBase, IFileDataStore
    where TSession : SessionBase<TSessionSnapshot>, new()
{
    /// <summary>
    /// Optional the snapshot that was originally loaded, for rollback purposes.
    /// </summary>
    private Option<TSessionSnapshot> _rollbackSnapshot = Option.None;

    /// <inheritdoc cref="IFileDataStore.LoadAsync(string)"/>
    public async Task<Result> LoadAsync(string filePath)
    {
        var session = new TSession();

        return await persistenceStrategy.LoadAsync(filePath)
            .Tap(session.Load)
            .Tap(_ => _cache = new ConcurrentDictionary<Type, object>(session.GetExposedEntities()))
            .Tap(snapshot => _rollbackSnapshot = snapshot);
    }

    /// <inheritdoc cref="IFileDataStore.PersistAsync(string)"/>
    public async Task<Result> PersistAsync(string filePath)
    {
        var session = new TSession();
        session.Load(_cache);
        var snapshot = session.ToSnapshot();
        return await persistenceStrategy.PersistAsync(snapshot, filePath)
            .Tap(() => _rollbackSnapshot = snapshot);
    }

    /// <inheritdoc cref="IFileDataStore.Rollback"/>
    public void Rollback()
    {
        if (!_rollbackSnapshot.TryGetValue(out var snapshot))
        {
            return;
        }
        var session = new TSession();
        session.Load(snapshot);
        _cache = new ConcurrentDictionary<Type, object>(session.GetExposedEntities());
    }

    /// <inheritdoc cref="InMemoryDataStore.GetSet{TEntity, TKey}"/>
    protected override Task<ConcurrentDictionary<TKey, TEntity>> GetSet<TEntity, TKey>()
    {
        if (!_rollbackSnapshot.HasValue)
        {
            throw new InvalidOperationException($"SessionDataStore is not yet initialized.");
        }

        if (!_cache.TryGetValue(typeof(TEntity), out var dict))
        {
            throw new InvalidOperationException($"Entity type '{typeof(TEntity)}' not found in cache.");
        }

        return Task.FromResult((ConcurrentDictionary<TKey, TEntity>) dict);
    }
}
