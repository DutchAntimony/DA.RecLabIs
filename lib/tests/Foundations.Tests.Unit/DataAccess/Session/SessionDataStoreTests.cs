using DA.Foundations.DataAccess.Persistence;
using DA.Foundations.DataAccess.Sessions;
using DA.Foundations.Examples;
using Foundations.Tests.Unit.Samples;

namespace Foundations.Tests.Unit.DataAccess.Session;

public class SessionDataStoreTests
{
    private readonly ISnapshotPersistenceStrategy<ExampleSessionSnapshot> _strategy = Substitute.For<ISnapshotPersistenceStrategy<ExampleSessionSnapshot>>();
    private readonly SessionDataStore<ExampleSession, ExampleSessionSnapshot> _store;

    public SessionDataStoreTests()
    {
        _store = new(_strategy);
    }

    [Fact]
    public async Task LoadAsync_ShouldPopulateCache()
    {
        var snapshot = new ExampleSessionSnapshot([new(Guid.NewGuid(), "Customer1"), new(Guid.NewGuid(), "Customer2")]);

        _strategy.LoadAsync("session.json")
            .Returns(Result.Success(snapshot));

        var result = await _store.LoadAsync("session.json");

        result.IsSuccess.ShouldBeTrue();
        var all = await _store.QueryAsync<CustomerType, CustomerId>();
        all.Count().ShouldBe(2);
        all.ShouldContain(c => c.Naam == "Customer1");
    }

    [Fact]
    public async Task PersistAsync_ShouldSaveSnapshotWithModifiedEntities()
    {
        var initial = Customer.CreateNew("Original");

        _strategy.LoadAsync("load.json")
            .Returns(Result.Success(new ExampleSessionSnapshot([])));

        await _store.LoadAsync("load.json"); // forces _activeSession to exist
        await _store.AddOrUpdateAsync<CustomerType, CustomerId>(initial);

        _strategy.PersistAsync(Arg.Any<ExampleSessionSnapshot>(), "save.json")
            .Returns(Result.Success());

        var result = await _store.PersistAsync("save.json");

        result.IsSuccess.ShouldBeTrue();
        await _strategy.Received(1).PersistAsync(Arg.Is<ExampleSessionSnapshot>(
            snapshot => snapshot.Customers.Any(c => c.Naam == "Original")
        ), "save.json");
    }

    [Fact]
    public async Task QueryAsync_Should_Throw_WhenStoreIsUnloaded()
    {
        var exception = await Should.ThrowAsync<InvalidOperationException>(async () => await _store.QueryAsync<CustomerType, CustomerId>());
        exception.Message.ShouldBe("SessionDataStore is not yet initialized.");
    }

    [Fact]
    public async Task QueryAsync_Should_Throw_WhenCollectionIsUnknown()
    {
        var snapshot = new ExampleSessionSnapshot([new(Guid.NewGuid(), "Customer1"), new(Guid.NewGuid(), "Customer2")]);

        _strategy.LoadAsync("session.json")
            .Returns(Result.Success(snapshot));

        await _store.LoadAsync("session.json");

        var testEntity = new ImmutableTestEntityType(new(Guid.NewGuid()), 1);

        var exception = await Should.ThrowAsync<InvalidOperationException>(async () => await _store.AddOrUpdateAsync<ImmutableTestEntityType, ImmutableTestEntityId>(testEntity));
        exception.Message.ShouldContain(nameof(ImmutableTestEntityType));
        exception.Message.ShouldContain("not found in cache");
    }

    [Fact]
    public async Task Rollback_Should_RestoreOriginalSnapshot()
    {
        var id = new CustomerId(Guid.NewGuid());

        var snapshot = new ExampleSessionSnapshot([new(id.Value, "Original")]);

        _strategy.LoadAsync("file.json").Returns(Result.Success(snapshot));

        await _store.LoadAsync("file.json");
        await _store.AddOrUpdateAsync<CustomerType, CustomerId>(Customer.CreateExisting(id, "Updated"));

        _store.Rollback();
        var current = await _store.FindByIdAsync<CustomerType, CustomerId>(id);

        current.ShouldNotBeNull();
        current.TryGetValue(out var actual).ShouldBeTrue();
        actual.Naam.ShouldBe("Original");
    }

    [Fact]
    public void Rollback_Should_DoNothing_IfCollectionIsNotInitialized()
    {
        Should.NotThrow(_store.Rollback);
    }
}
