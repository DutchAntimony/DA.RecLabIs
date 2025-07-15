namespace DA.Messaging.Pagination;

public static class PagingExtensions
{
    /// <summary>
    /// Select a result page with a size and number from the collection.
    /// </summary>
    /// <param name="collection">The collection that calls this method.</param>
    /// <param name="pagingInformation">Information regarding page number and page size.</param>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <returns>An IEnumerable{T} that contains the part of the collection that is on the specified page.</returns>
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> collection, PagingInformation pagingInformation) =>
        collection.Paginate(pagingInformation.Page, pagingInformation.QuantityPerPage);

    /// <summary>
    /// Select a result page with a size and number from the collection.
    /// </summary>
    /// <param name="collection">The collection that calls this method.</param>
    /// <param name="page">The page number to return.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <returns>An IEnumerable{T} that contains the part of the collection that is on the specified page.</returns>
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> collection, int page, int pageSize) =>
        collection.Skip((page - 1) * pageSize).Take(pageSize);

    /// <summary>
    /// Select a result page with a size and number from the collection and wrap it in a <see cref="PaginatedCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection that calls this method.</param>
    /// <param name="pagingInformation">Information regarding page number and page size.</param>
    /// <returns>A <see cref="PaginatedCollection{TInner}"/> that wraps the selected page of the collection.</returns>
    public static PaginatedCollection<T> ToPaginatedCollection<T>(this IEnumerable<T> collection, PagingInformation pagingInformation)
    {
        var items = collection.Paginate(pagingInformation);
        return new PaginatedCollection<T>(items, collection.Count(), pagingInformation);
    }

    /// <summary>
    /// Select a result page with a size and number from the collection and wrap it in a <see cref="PaginatedCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection that calls this method.</param>
    /// <param name="page">The page number to return.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A <see cref="PaginatedCollection{TInner}"/> that wraps the selected page of the collection.</returns>
    public static PaginatedCollection<T> ToPaginatedCollection<T>(this IEnumerable<T> collection, int page, int pageSize)
    {
        var pagingInformation = new PagingInformation(page, pageSize);
        return collection.ToPaginatedCollection(pagingInformation);
    }
}
