namespace DA.Results.Errors;

/// <summary>
/// Respresents an error indicating that no results were found for a specific type based on a search description.
/// </summary>
/// <remarks>This type encapsulates any error that occures when no results of a given type where found.
/// This can usually happen when querying a database or searching through a collection. </remarks>
/// <param name="Type">The type of the data that was queried.</param>
/// <param name="SearchDescription">Description of what was searched for.</param>
public sealed record class NotFoundError(string Type, string SearchDescription) : Error($"No results of '{Type}' where found given {SearchDescription}");

