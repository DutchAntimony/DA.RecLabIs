using DA.Foundations.DataAccess.Sessions;
using DA.Foundations.Entities;
using DA.Foundations.Examples;
using System.Collections.Concurrent;

namespace Foundations.Tests.Unit.DataAccess.Session;
public class SessionBaseTests
{
    private readonly TestSession _session = new();

    [Fact]
    public void ReadExposed_Should_Return_ConcurrentDictionary_When_Type_Is_Correct()
    {
        var customer = Customer.CreateNew("Naam");
        _session.Expose<CustomerType, CustomerId>([customer]);
        var exposed = _session.GetExposedEntities();

        var result = _session.ReadExposed<CustomerType, CustomerId>(exposed);

        result.ShouldNotBeNull();
        result.ShouldContainKey(customer.Id);
        result[customer.Id].Naam.ShouldBe("Naam");
    }

    [Fact]
    public void ReadExposed_Should_Throw_When_Type_Is_Not_Found()
    {
        var exposed = new Dictionary<Type, object>(); // empty

        var ex = Should.Throw<InvalidOperationException>(() =>
            _session.ReadExposed<CustomerType, CustomerId>(exposed));

        ex.Message.ShouldContain("No data found for type");
    }

    [Fact]
    public void ReadExposed_Should_Throw_When_Type_Is_Not_Correct_Dictionary()
    {
        var wrongDict = new Dictionary<CustomerId, CustomerType>(); // not concurrent
        var exposed = new Dictionary<Type, object>
        {
            [typeof(CustomerType)] = wrongDict
        };

        var ex = Should.Throw<InvalidOperationException>(() =>
            _session.ReadExposed<CustomerType, CustomerId>(exposed));

        ex.Message.ShouldContain("Invalid dictionary for type");
    }

    private sealed class TestSession : SessionBase<string>
    {
        public override string ToSnapshot() => throw new NotImplementedException();
        public override void Load(string snapshot) => throw new NotImplementedException();
        public override void Load(IReadOnlyDictionary<Type, object> entities) => throw new NotImplementedException();

        // Make Expose public for testability
        public new void Expose<TEntity, TKey>(IEnumerable<TEntity> entities)
            where TEntity : IEntity<TKey>
            where TKey : struct, IEntityKey
            => base.Expose<TEntity, TKey>(entities);

        public new ConcurrentDictionary<TKey, TEntity> ReadExposed<TEntity, TKey>(IReadOnlyDictionary<Type, object> exposed)
            where TEntity : IEntity<TKey>
            where TKey : struct, IEntityKey
            => base.ReadExposed<TEntity, TKey>(exposed);

        public new IReadOnlyDictionary<Type, object> GetExposedEntities()
            => base.GetExposedEntities();
    }
}
