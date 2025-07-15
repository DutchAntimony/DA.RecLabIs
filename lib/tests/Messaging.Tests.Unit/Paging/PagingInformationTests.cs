namespace Messaging.Tests.Unit.Paging;

public class PagingInformationTests
{
    [Fact]
    public void PagingInformation_ShouldHaveCorrectValues()
    {
        var pagingInfo = new PagingInformation(2, 10);
        pagingInfo.Page.ShouldBe(2);
        pagingInfo.QuantityPerPage.ShouldBe(10);
    }

    [Fact]
    public void PagingInformation_PageExist_ShouldReturnCorrectly()
    {
        var paging = new PagingInformation(2, 10);
        paging.PageExist(25).ShouldBeTrue();  // page 2 exists
        paging.PageExist(15).ShouldBeTrue();  // page 2 exists
        paging.PageExist(10).ShouldBeFalse(); // page 2 starts at index 11
    }

    [Fact]
    public void PagingInformation_All_ShouldIncludeAllItems()
    {
        var paging = PagingInformation.All;
        var items = Enumerable.Range(1, 100);
        var paged = items.Paginate(paging);

        paged.Count().ShouldBe(100);
    }

    [Fact]
    public void PagingInformation_Default_ShouldReturnFirstPageOf25()
    {
        var paging = PagingInformation.Default;
        var items = Enumerable.Range(1, 100);
        var paged = items.Paginate(paging).ToList();

        paged.Count.ShouldBe(25);
        paged.First().ShouldBe(1);
        paged.Last().ShouldBe(25);
    }
}