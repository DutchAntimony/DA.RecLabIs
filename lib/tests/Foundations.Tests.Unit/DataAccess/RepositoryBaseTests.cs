using DA.Foundations.DataAccess;
using DA.Foundations.Examples;
using DA.Optional;
using Foundations.Tests.Unit.Helpers;
using NSubstitute;

namespace Foundations.Tests.Unit.DataAccess;

public class RepositoryBaseTests
{
    private readonly IDataStore _dataStore = Substitute.For<IDataStore>();
    private readonly TestRepository _repository;

    public RepositoryBaseTests()
    {
        _repository = new TestRepository(_dataStore);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnDataStoreResult_AsIEnumerable()
    {
        var customer = Customer.CreateNew("naam");
        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer}.AsQueryable());

        var result = await _repository.GetAllAsync();

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
        result.ShouldContain(customer);
    }

    [Fact]
    public async Task GetWhereAsync_Should_ReturnAllDataMatchingThePredicate()
    {
        var customer1 = Customer.CreateNew("NL-1");
        var customer2 = Customer.CreateNew("NL-2");
        var customer3 = Customer.CreateNew("B-1");

        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer1, customer2, customer3 }.AsQueryable());

        var result = await _repository.GetWhereAsync(c => c.Naam.StartsWith("NL"));

        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.ShouldContain(customer1);
        result.ShouldNotContain(customer3);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnSuccess_WhenIdIsFound()
    {
        var customer = Customer.CreateNew("naam");

        _dataStore.FindByIdAsync<CustomerType, CustomerId>(customer.Id, Arg.Any<CancellationToken>())
            .Returns(customer);

        var result = await _repository.GetByIdAsync(customer.Id);
        result.ShouldBeSuccessWithValue(customer);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnFailure_WhenIdIsNotFound()
    {
        var fakeId = new CustomerId() { Value = Guid.NewGuid() };

        _dataStore.FindByIdAsync<CustomerType, CustomerId>(fakeId, Arg.Any<CancellationToken>())
            .Returns(Option.None);

        var result = await _repository.GetByIdAsync(fakeId);
        result.ShouldBeFailureOfType<NotFoundError>("Customer");
    }

    [Fact]
    public async Task ExistsAsync_Should_ReturnTrue_WhenIdIsFound()
    {
        var customer = Customer.CreateNew("naam");

        _dataStore.FindByIdAsync<CustomerType, CustomerId>(customer.Id, Arg.Any<CancellationToken>())
            .Returns(customer);

        (await _repository.ExistsAsync(customer.Id)).ShouldBeTrue();
    }

    [Fact]
    public async Task ExistsAsync_Should_ReturnFalse_WhenIdIsNotFound()
    {
        var fakeId = new CustomerId() { Value = Guid.NewGuid() };

        _dataStore.FindByIdAsync<CustomerType, CustomerId>(fakeId, Arg.Any<CancellationToken>())
            .Returns(Option.None);

        (await _repository.ExistsAsync(fakeId)).ShouldBeFalse();
    }

    [Fact]
    public async Task CountAsync_Should_ReturnCountOfAllData()
    {
        var customer1 = Customer.CreateNew("NL-1");
        var customer2 = Customer.CreateNew("NL-2");
        var customer3 = Customer.CreateNew("B-1");

        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer1, customer2, customer3 }.AsQueryable());

        (await _repository.CountAsync()).ShouldBe(3);
    }

    [Fact]
    public async Task CountAsync_Should_ReturnCountOfAllDataMatchingThePredicate()
    {
        var customer1 = Customer.CreateNew("NL-1");
        var customer2 = Customer.CreateNew("NL-2");
        var customer3 = Customer.CreateNew("B-1");

        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer1, customer2, customer3 }.AsQueryable());

        (await _repository.CountAsync(c => c.Naam.StartsWith("NL"))).ShouldBe(2);
    }

    [Fact]
    public async Task GetSingleAsync_Should_ReturnCustomer_WhenPredicateIsMatchedBy1Entity()
    {
        var customer1 = Customer.CreateNew("NL-1");
        var customer2 = Customer.CreateNew("NL-2");
        var customer3 = Customer.CreateNew("B-1");

        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer1, customer2, customer3 }.AsQueryable());

        var result = await _repository.GetSingleAsync(c => c.Naam.StartsWith("B"));
        result.ShouldBeSuccessWithValue(customer3);
    }

    [Fact]
    public async Task GetSingleAsync_Should_ReturnNotFoundError_WhenPredicateIsNotMatched()
    {
        var customer1 = Customer.CreateNew("NL-1");
        var customer2 = Customer.CreateNew("NL-2");
        var customer3 = Customer.CreateNew("B-1");

        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer1, customer2, customer3 }.AsQueryable());

        var result = await _repository.GetSingleAsync(c => c.Naam.StartsWith("D"));
        result.ShouldBeFailureOfType<NotFoundError>(nameof(CustomerType));
    }

    [Fact]
    public async Task GetSingleAsync_Should_ReturnUnexpectedError_WhenPredicateIsMatchedMoreThanOnce()
    {
        var customer1 = Customer.CreateNew("NL-1");
        var customer2 = Customer.CreateNew("NL-2");
        var customer3 = Customer.CreateNew("B-1");

        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer1, customer2, customer3 }.AsQueryable());

        var result = await _repository.GetSingleAsync(c => c.Naam.StartsWith("NL"));
        result.ShouldBeFailureOfType<UnexpectedError>("More than one entity found");
    }

    [Fact]
    public async Task GetSingleOrNoneAsync_Should_ReturnOptionOfEntity_WhenSingleIsFound()
    {
        var customer1 = Customer.CreateNew("NL-1");
        var customer2 = Customer.CreateNew("NL-2");
        var customer3 = Customer.CreateNew("B-1");

        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer1, customer2, customer3 }.AsQueryable());

        var result = await _repository.GetSingleOrNoneAsync(c => c.Naam.StartsWith("B"));
        result.TryGetValue(out var value).ShouldBeTrue();
        value.ShouldBe(customer3);
    }

    [Fact]
    public async Task GetSingleOrNoneAsync_Should_ReturnOptionNone_WhenNotFound()
    {
        var customer1 = Customer.CreateNew("NL-1");
        var customer2 = Customer.CreateNew("NL-2");
        var customer3 = Customer.CreateNew("B-1");

        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer1, customer2, customer3 }.AsQueryable());

        var result = await _repository.GetSingleOrNoneAsync(c => c.Naam.StartsWith("D"));
        result.HasValue.ShouldBeFalse();
    }

    [Fact]
    public async Task GetSingleOrNoneAsync_Should_Throw_WhenMoreThan1IsFound()
    {
        var customer1 = Customer.CreateNew("NL-1");
        var customer2 = Customer.CreateNew("NL-2");
        var customer3 = Customer.CreateNew("B-1");

        _dataStore.QueryAsync<CustomerType, CustomerId>(Arg.Any<CancellationToken>())
            .Returns(new List<CustomerType>() { customer1, customer2, customer3 }.AsQueryable());

        await Should.ThrowAsync<InvalidOperationException>(async () => 
            await _repository.GetSingleOrNoneAsync(c => c.Naam.StartsWith("NL")));
    }

    [Fact]
    public async Task AddAsync_Should_ReturnSuccess_WhenItemIsNotYetInCollection()
    {
        var customer = Customer.CreateNew("NL-1");

        _dataStore.FindByIdAsync<CustomerType, CustomerId>(customer.Id, Arg.Any<CancellationToken>())
            .Returns(Option.None); // customer does not yet exist.
        var result = await _repository.AddAsync(customer);

        result.ShouldBeSuccessWithValue(customer);
        await _dataStore.Received(1).AddOrUpdateAsync<CustomerType, CustomerId>(customer);

    }

    [Fact]
    public async Task AddAsync_Should_ReturnFailure_WhenItemIsAlreadyPresentInCollection()
    {
        var customer = Customer.CreateNew("NL-1");

        _dataStore.FindByIdAsync<CustomerType, CustomerId>(customer.Id, Arg.Any<CancellationToken>())
            .Returns(customer); // customer already exists.

        var result = await _repository.AddAsync(customer);
        result.ShouldBeFailureOfType<DomainError>(customer.Id.Value.ToString());

        await _dataStore.DidNotReceive().AddOrUpdateAsync<CustomerType, CustomerId>(customer);
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnSuccess_WhenItemWithIdIsAlreadyPresent()
    {
        var customer = Customer.CreateNew("NL-1");
        var updated = customer with { Naam = "Updated" };

        _dataStore.FindByIdAsync<CustomerType, CustomerId>(customer.Id, Arg.Any<CancellationToken>())
            .Returns(customer); // customer already exists.

        var result = await _repository.UpdateAsync(updated);
        result.ShouldBeSuccessWithValue(updated);

        await _dataStore.Received(1).AddOrUpdateAsync<CustomerType, CustomerId>(updated);
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnFailure_WhenItemWithIdIsNotPresent()
    {
        var updated = Customer.CreateNew("NL-1");

        _dataStore.FindByIdAsync<CustomerType, CustomerId>(updated.Id, Arg.Any<CancellationToken>())
            .Returns(Option.None); // customer already exists.

        var result = await _repository.UpdateAsync(updated);
        result.ShouldBeFailureOfType<NotFoundError>(nameof(CustomerType));

        await _dataStore.DidNotReceive().AddOrUpdateAsync<CustomerType, CustomerId>(updated);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnSuccess_WhenItemIsPresent()
    {
        var customer = Customer.CreateNew("NL-1");
        _dataStore.RemoveAsync<CustomerType, CustomerId>(customer.Id).Returns(Result.Success());

        var result = await _repository.DeleteAsync(customer);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnSuccess_WhenItemIsNotPresent()
    {
        var customer = Customer.CreateNew("NL-1");
        var error = new NotFoundError(nameof(CustomerType), "ById:");
        _dataStore.RemoveAsync<CustomerType, CustomerId>(customer.Id).Returns(error);

        var result = await _repository.DeleteAsync(customer);
        result.ShouldBeFailureWithError(error);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnSuccess_WhenItemIdIsPresent()
    {
        var customerId = new CustomerId(Guid.CreateVersion7());
        _dataStore.RemoveAsync<CustomerType, CustomerId>(customerId).Returns(Result.Success());

        var result = await _repository.DeleteAsync(customerId);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnSuccess_WhenItemIdIsNotPresent()
    {
        var customerId = new CustomerId(Guid.CreateVersion7());
        var error = new NotFoundError(nameof(CustomerType), "ById:");
        _dataStore.RemoveAsync<CustomerType, CustomerId>(customerId).Returns(error);

        var result = await _repository.DeleteAsync(customerId);
        result.ShouldBeFailureWithError(error);
    }

    private class TestRepository(IDataStore dataStore) : RepositoryBase<CustomerType, CustomerId>(dataStore) { }
}
