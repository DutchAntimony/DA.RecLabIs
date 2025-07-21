using DA.Foundations.DataAccess.Sessions;
using DA.Results;

namespace DA.Foundations.DataAccess.Persistence;

/// <summary>
/// Strategy to store a Snapshot of a <see cref="SessionBase{TSnapshot}"/> to a file. 
/// </summary>
/// <typeparam name="TSnapshot">The type of the snapshot of the <see cref="SessionBase{TSessionSnapshot}"/></typeparam>
public interface ISnapshotPersistenceStrategy<TSnapshot>
{
    /// <summary>
    /// Load the entire snapshot from the provided file.
    /// </summary>
    /// <param name="filePath">The location of the file to load from.</param>
    /// <returns>A new instance of a <see cref="TSnapshot"/></returns>
    Task<Result<TSnapshot>> LoadAsync(string filePath);

    /// <summary>
    /// Persist the entiry provided <paramref name="snapshot"/> to the provided <paramref name="filePath"/>
    /// </summary>
    /// <param name="snapshot">The snapshot to persist.</param>
    /// <param name="filePath">The location where to save the created file.</param>
    Task<Result> PersistAsync(TSnapshot snapshot, string filePath);
}
