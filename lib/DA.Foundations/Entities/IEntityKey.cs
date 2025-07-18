namespace DA.Foundations.Entities;

/// <summary>
/// Strongly typed entity key interface.
/// </summary>
public interface IEntityKey
{
    /// <summary>
    /// Internal value of the entity key.
    /// Preferable use <code>Guid.CreateVersion7()</code> for the construction
    /// of lexographically sortable keys.
    /// </summary>
    /// <remarks>
    /// The Value is settable during init, to make creation of new keys possible.</remarks>
    Guid Value { get; init; }
}
