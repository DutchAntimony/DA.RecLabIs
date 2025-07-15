namespace DA.Messaging.Pagination;

/// <summary>
/// Paginated collection of items that are returned from a query.
/// </summary>
/// <typeparam name="TInner">The type of the inner items that are paged.</typeparam>
public class PaginatedCollection<TInner>(IEnumerable<TInner> items, int totalCount, PagingInformation pagingInfo)
{
    /// <summary>
    /// The collection of items that got paginated.
    /// </summary>
    public IEnumerable<TInner> Items { get; } = items;

    /// <summary>
    /// The total number of entries this collection contains.
    /// Not all are shown, just the total size of the collection.
    /// </summary>
    public int TotalCount { get; } = totalCount;

    /// <summary>
    /// The current page that is displayed.
    /// </summary>
    public int Page { get; } = pagingInfo.Page;

    /// <summary>
    /// The number of entries per page.
    /// </summary>
    public int PageSize { get; } = pagingInfo.QuantityPerPage;

    /// <summary>
    /// Has the current selection of inner responses a next page that can be displayed.
    /// </summary>
    public bool HasNextPage => Page * PageSize < TotalCount;

    /// <summary>
    /// Has the current selection of inner responses a previous page that can be displayed.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// The total number of pages this collection contains.
    /// </summary>
    public int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);
}
