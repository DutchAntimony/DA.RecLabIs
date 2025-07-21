using DA.Results;

namespace DA.Foundations.DataAccess;

/// <summary>
/// Methods for managing the <see cref="IDataStore"> from a file.
/// </summary>
public interface IFileDataStore : IDataStore
{
    /// <summary>
    /// Load the datastore from a file by the provided <param name="filePath" />
    /// </summary>
    /// <param name="filePath">The path of the file to load.</param>
    /// <returns>A <see cref="Result"> indicating if the loaded succeeded.</returns>
    public Task<Result> LoadAsync(string filePath);

    /// <summary>
    /// Persist the contents of the datastore to a file at the provided path.
    /// </summary>
    /// <param name="filePath">The path where to save the new file.</param>
    /// <returns>A <see cref="Result"/> indicating if the saving succeeded.</returns>
    public Task<Result> PersistAsync(string filePath);

    /// <summary>
    /// Rollback the contents of the store to its original position after the last <see cref="LoadAsync(string)"/>
    /// </summary>
    public void Rollback();
}
