namespace DA.Foundations.DependencyInjection;

/// <summary>
/// Pre configured persistence mode for use in <see cref="SessionOptions"/>
/// </summary>
public enum PersistenceMode
{
    JsonFile,
    EncryptedJsonFile,
    ZippedJsonFile,
    EncryptedZippedJsonFile
}


