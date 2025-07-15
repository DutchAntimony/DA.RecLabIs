namespace Messaging.Tests.Unit.Paging;

public class PaginatedCollectionTests
{
    [Fact]
    public void ToPaginatedCollection_PageCountShouldBeCeiling()
    {
        var items = Enumerable.Range(1, 25);
        var paged = items.ToPaginatedCollection(1, 6); // should result in 5 pages

        paged.PageCount.ShouldBe(5);
        paged.HasPreviousPage.ShouldBeFalse();
    }

    [Fact]
    public void PaginatedCollection_CanBeConstructedUsingConstructor()
    {
        var items = Enumerable.Range(1, 25);
        var paged = new PaginatedCollection<int>(items, items.Count(), new PagingInformation(1, 6));

        paged.PageCount.ShouldBe(5);
        paged.HasPreviousPage.ShouldBeFalse();
    }
}