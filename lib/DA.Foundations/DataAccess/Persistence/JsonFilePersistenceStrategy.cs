using DA.Results;
using System.Text.Json;

namespace DA.Foundations.DataAccess.Persistence;

/// <summary>
/// Persistence strategy that stores the snapshot in a single json file, without any further modifications.<code />
/// <see cref="ISnapshotPersistenceStrategy{TSnapshot}"/>: <inheritdoc cref="ISnapshotPersistenceStrategy{TSnapshot}" />
/// </summary>
/// <remarks>Todo: improve result returning</remarks>
internal sealed class JsonFilePersistenceStrategy<TSnapshot> : ISnapshotPersistenceStrategy<TSnapshot>
{
    private readonly JsonSerializerOptions _serializerOptions = 
        new() { WriteIndented = true };

    public async Task<Result<TSnapshot>> LoadAsync(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<TSnapshot>(stream)
            ?? throw new InvalidOperationException("Failed to deserialize snapshot.");
    }

    public async Task<Result> PersistAsync(TSnapshot snapshot, string filePath)
    {
        using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, snapshot, _serializerOptions);
        return Result.Success();
    }
}