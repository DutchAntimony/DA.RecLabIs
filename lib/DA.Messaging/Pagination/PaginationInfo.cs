namespace DA.Messaging.Pagination;

/// <summary>
/// Combination of Page and QuantityPerPage which together describe a page that results from paginating a query.
/// </summary>
public record PagingInformation(int Page, int QuantityPerPage)
{
    /// <summary>
    /// Static pagingInformation on which all entries are displayed on a single page.
    /// May cause performance issues if view models must be constructed for many items.
    /// </summary>
    public static PagingInformation All => new(1, int.MaxValue);

    /// <summary>
    /// Static pagingInformation with reasonable defaults (25 items per page) and returning the first page.
    /// </summary>
    public static PagingInformation Default => new(1, 25);

    /// <summary>
    /// The index of what will be the first entry on the provided page.
    /// </summary>
    public int IndexOfFirstEntryOnPage => ((Page - 1) * QuantityPerPage) + 1;

    /// <summary>
    /// Does the page provided by this paging information exist provided the total size of the collection.
    /// </summary>
    /// <param name="totalEntries">The total number of entries the collection contains.</param>
    /// <returns>True when the total number of entries is greater than the index of the first entry on this page.</returns>
    public bool PageExist(int totalEntries) => IndexOfFirstEntryOnPage <= totalEntries;
};