using DA.Optional;

namespace DA.Foundations.DependencyInjection;

/// <summary>
/// Options to register a Session based storage.
/// </summary>
public sealed class SessionOptions
{
    /// <summary>
    /// The mode used to persist the data in the session.
    /// </summary>
    public PersistenceMode Mode { get; set; } = PersistenceMode.JsonFile;

    /// <summary>
    /// Optional an Encryption key if the <see cref="PersistenceMode"/> requires one.
    /// </summary>
    /// <remarks> Of the pre configured persistence modes, only <see cref="PersistenceMode.EncryptedJsonFile"/> and <see cref="PersistenceMode.EncryptedZippedJsonFile"/> require a key.
    /// Configuring a key when not using an encrypted persistence mode does nothing with the provided key. </remarks>
    public Option<string> EncryptionKey { get; set; } = Option.None;
}


