namespace Messaging.Tests.Unit.Paging;
public class PagingExtensionsTests
{
    [Fact]
    public void Paginate_ShouldReturnCorrectPage()
    {
        var items = Enumerable.Range(1, 100);
        var paged = items.Paginate(2, 10).ToList();

        paged.Count.ShouldBe(10);
        paged.First().ShouldBe(11);
        paged.Last().ShouldBe(20);
    }

    [Fact]
    public void Paginate_ShouldReturnCorrectPage_WhenProvidingPagingInfo()
    {
        var items = Enumerable.Range(1, 100);
        var paged = items.Paginate(new PagingInformation(2, 10)).ToList();

        paged.Count.ShouldBe(10);
        paged.First().ShouldBe(11);
        paged.Last().ShouldBe(20);
    }

    [Fact]
    public void Paginate_FirstPage_ShouldStartAtOne()
    {
        var items = Enumerable.Range(1, 10);
        var paged = items.Paginate(1, 5).ToList();

        paged.ShouldBe(new[] { 1, 2, 3, 4, 5 });
    }

    [Fact]
    public void Paginate_FirstPage_ShouldStartAtOne_WhenProvidingPagingInformation()
    {
        var items = Enumerable.Range(1, 10);
        var paged = items.Paginate(new PagingInformation(1, 5)).ToList();

        paged.ShouldBe(new[] { 1, 2, 3, 4, 5 });
    }

    [Fact]
    public void Paginate_PageOutOfBounds_ShouldReturnEmpty()
    {
        var items = Enumerable.Range(1, 10);
        var paged = items.Paginate(5, 10);

        paged.ShouldBeEmpty();
    }

    [Fact]
    public void Paginate_PageOutOfBounds_ShouldReturnEmpty_WhenProvidingPagingInformation()
    {
        var items = Enumerable.Range(1, 10);
        var paged = items.Paginate(new PagingInformation(5,10));

        paged.ShouldBeEmpty();
    }

    [Fact]
    public void ToPaginatedCollection_ShouldWrapPaginatedPage()
    {
        var items = Enumerable.Range(1, 30);
        var paged = items.ToPaginatedCollection(2, 10);

        paged.Page.ShouldBe(2);
        paged.PageSize.ShouldBe(10);
        paged.TotalCount.ShouldBe(30);
        paged.HasNextPage.ShouldBeTrue();
        paged.HasPreviousPage.ShouldBeTrue();
        paged.Items.ShouldBe(new[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });
        paged.PageCount.ShouldBe(3);
    }

    [Fact]
    public void ToPaginatedCollection_ShouldWrapPaginatedPage_WhenProvidingPagingInformation()
    {
        var items = Enumerable.Range(1, 30);
        var paged = items.ToPaginatedCollection(new PagingInformation(2, 10));

        paged.Page.ShouldBe(2);
        paged.PageSize.ShouldBe(10);
        paged.TotalCount.ShouldBe(30);
        paged.HasNextPage.ShouldBeTrue();
        paged.HasPreviousPage.ShouldBeTrue();
        paged.Items.ShouldBe(new[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });
        paged.PageCount.ShouldBe(3);
    }
}
