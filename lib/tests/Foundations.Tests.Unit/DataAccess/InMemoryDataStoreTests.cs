using DA.Foundations.DataAccess;
using DA.Foundations.Examples;
using Foundations.Tests.Unit.Helpers;
using Foundations.Tests.Unit.Samples;

namespace Foundations.Tests.Unit.DataAccess;

public class InMemoryDataStoreTests
{
    private readonly InMemoryDataStore _dataStore = new();

    [Fact]
    public async Task QueryAsync_Should_ReturnAllEntitiesInSet()
    {
        var customer1 = await Insert("customer1");
        _ = await Insert("customer2");

        var result = await _dataStore.QueryAsync<CustomerType, CustomerId>();
        result.Count().ShouldBe(2);
        result.ShouldContain(customer1);
    }

    [Fact]
    public async Task QueryAsync_Should_ReturnEmptyCollection_IfEntityDoesNotExist()
    {
        var result = await _dataStore.QueryAsync<CustomerType, CustomerId>();
        result.Count().ShouldBe(0);
    }

    [Fact]
    public async Task FindByIdAsync_Should_ReturnSome_IfFound()
    {
        var customer = await Insert();

        var result = await _dataStore.FindByIdAsync<CustomerType, CustomerId>(customer.Id);

        result.HasValue.ShouldBeTrue();
    }

    [Fact]
    public async Task FindByIdAsync_Should_ReturnNone_IfNotFound()
    {
        var customer = await Insert();
        var fakeId = new CustomerId() { Value = Guid.NewGuid() };

        var result = await _dataStore.FindByIdAsync<CustomerType, CustomerId>(fakeId);

        result.HasValue.ShouldBeFalse();
    }

    [Fact]
    public async Task AddOrUpdateAsync_Should_Add_NewItem()
    {
        var customer = Customer.CreateNew("Naam");

        await _dataStore.AddOrUpdateAsync<CustomerType, CustomerId>(customer, CancellationToken.None);

        await StoreShouldContain(customer);
    }

    [Fact]
    public async Task AddOrUpdateAsync_Should_ReplaceExistingItem()
    {
        var customer = await Insert("original");
        var updated = customer with { Naam = "Updated" };

        await _dataStore.AddOrUpdateAsync<CustomerType, CustomerId>(updated, CancellationToken.None);

        await StoreShouldContain(updated);
    }

    [Fact]
    public async Task RemoveAsync_Should_DeleteEntity()
    {
        var customer = await Insert();

        var result = await _dataStore.RemoveAsync<CustomerType, CustomerId>(customer.Id, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();

        await StoreShouldNotContain(customer.Id);
    }

    [Fact]
    public async Task RemoveAsync_Should_ReturnFailure_IfEntityDoesNotExist()
    {
        var fakeId = new CustomerId() { Value = Guid.NewGuid() };

        var result = await _dataStore.RemoveAsync<CustomerType, CustomerId>(fakeId, CancellationToken.None);
        result.ShouldBeFailureOfType<NotFoundError>("Customer");

        await StoreShouldNotContain(fakeId); // Store should still not contain this Id.
    }


    [Fact]
    public async Task Clear_Should_RemoveAllEntitiesFromCollection()
    {
        var customer1 = await Insert("Customer1");
        _ = await Insert("Customer2");
        _ = await Insert("Customer3");

        await _dataStore.Clear<CustomerType>();

        await StoreShouldNotContain(customer1.Id);
        
        (await _dataStore.QueryAsync<CustomerType, CustomerId>()).Count().ShouldBe(0);
    }

    [Fact]
    public async Task Clear_Should_NotRemoveEntitiesFromDifferentTypeFromCollection()
    {
        var customer = await Insert();

        await _dataStore.Clear<ImmutableTestEntityType>();

        await StoreShouldContain(customer);

        (await _dataStore.QueryAsync<CustomerType, CustomerId>()).Count().ShouldBe(1);
    }

    private async Task<CustomerType> Insert(string naam = "naam")
    {
        var customer = Customer.CreateNew(naam);
        await _dataStore.AddOrUpdateAsync<CustomerType, CustomerId>(customer, CancellationToken.None);

        return customer;
    }

    private async Task StoreShouldContain(CustomerType expected)
    {
        var result = await _dataStore.FindByIdAsync<CustomerType, CustomerId>(expected.Id, CancellationToken.None);
        result.ShouldNotBeNull();
        result.TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldBe(expected);
    }

    private async Task StoreShouldNotContain(CustomerId id)
    {
        var result = await _dataStore.FindByIdAsync<CustomerType, CustomerId>(id, CancellationToken.None);
        result.HasValue.ShouldBeFalse();
    }
}
