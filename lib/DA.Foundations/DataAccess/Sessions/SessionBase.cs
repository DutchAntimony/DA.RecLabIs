using DA.Foundations.Entities;
using System.Collections.Concurrent;

namespace DA.Foundations.DataAccess.Sessions;

/// <summary>
/// Base class for a Session, a place where all related entities are stored.
/// A Session works together with a <typeparamref name="TSessionSnapshot"/>,
/// which is a serializable class that can be persisted by an <see cref="ISnapshotPersistenceStrategy{TSessionSnapshot}"/>.
/// </summary>
/// <typeparam name="TSessionSnapshot">The serializable variant of this data.</typeparam>
public abstract class SessionBase<TSessionSnapshot>
{
    private readonly Dictionary<Type, object> _exposedEntities = [];
    /// <summary>
    /// Collection of all entities that are exposed to the <see cref="SessionDataStore{TSession, TSessionSnapshot}"/>.
    /// Entities can be exposed using the <see cref="Expose{TEntity, TKey}(Dictionary{TKey, TEntity})"/> method.
    /// </summary>
    internal IReadOnlyDictionary<Type, object> GetExposedEntities() => _exposedEntities.AsReadOnly();

    /// <summary>
    /// Expose a collection of Entities to be used in the <see cref="GetExposedEntities"/> method.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to expose.</typeparam>
    /// <typeparam name="TKey">The corresponding key type of the entity.</typeparam>
    /// <param name="entities"></param>
    protected void Expose<TEntity, TKey>(IEnumerable<TEntity> entities)
        where TEntity : IEntity<TKey>
        where TKey : struct, IEntityKey
    {
        _exposedEntities[typeof(TEntity)] = new ConcurrentDictionary<TKey, TEntity>(entities.ToDictionary(e => e.Id, e => e));
    }

    /// <summary>
    /// Read an already exposed entity back to a <see cref="Dictionary{TKey, TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that was exposed an possibly editted.</typeparam>
    /// <typeparam name="TKey">The corresponding key type of the entity.</typeparam>
    protected ConcurrentDictionary<TKey, TEntity> ReadExposed<TEntity, TKey>(IReadOnlyDictionary<Type, object> exposed)
    where TEntity : IEntity<TKey>
    where TKey : struct, IEntityKey
    {
        if (!exposed.TryGetValue(typeof(TEntity), out var obj))
        {
            throw new InvalidOperationException($"No data found for type '{typeof(TEntity)}'.");
        }

        if (obj is not ConcurrentDictionary<TKey, TEntity> casted)
        {
            throw new InvalidOperationException($"Invalid dictionary for type '{typeof(TEntity)}'.");
        }

        Expose<TEntity, TKey>(casted.Values); // update the content in the _exposedEntities if for some reason this Session is reloaded but not from file.
        return casted;
    }

    /// <summary>
    /// Mapping logic to serialize the Session back a <typeparamref name="TSessionSnapshot"/>.
    /// </summary>
    public abstract TSessionSnapshot ToSnapshot();

    /// <summary>
    /// Mapping logic to fill the Session from a <typeparamref name="TSessionSnapshot"/>.
    /// </summary>
    /// <remarks> Use the <see cref="Expose{TEntity, TKey}(Dictionary{TKey, TEntity})"/> method on every loaded entity to expose it to the store.</remarks>
    public abstract void Load(TSessionSnapshot snapshot);

    /// <summary>
    /// Method to import the concrete dictionaries back from the exposed state.
    /// </summary>
    /// <remarks> Use the <see cref="ReadExposed{TEntity, TKey}"/> helper function for every collection in the Session.</remarks>
    public abstract void Load(IReadOnlyDictionary<Type, object> entities);
}
