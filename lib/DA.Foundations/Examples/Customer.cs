using DA.Foundations.DataAccess;
using DA.Foundations.DataAccess.Repositories;
using DA.Foundations.DataAccess.Sessions;
using DA.Foundations.Entities;
using DA.Foundations.Entities.Immutable;
using DA.Results;
using DA.Results.Extensions;

namespace DA.Foundations.Examples;

public readonly record struct CustomerId(Guid Value) : IEntityKey;


public sealed record CustomerType(string Naam) : Entity<CustomerId>()
{
    internal CustomerType(CustomerId id, string naam) : this(naam)
    {
        Id = id;
    }
}


public static class Customer
{
    public static CustomerType CreateNew(string naam)
    {
        return new CustomerType(naam);
    }

    public static CustomerType CreateExisting(CustomerId id, string naam)
    {
        return new CustomerType(id, naam);
    }
}

public sealed record CustomerDto(Guid Id, string Naam);

public record ExampleSessionSnapshot(IEnumerable<CustomerDto> Customers);

public class ExampleSession : SessionBase<ExampleSessionSnapshot>
{
    private IEnumerable<CustomerType> _customers = [];

    public override ExampleSessionSnapshot ToSnapshot()
    {
        var customerDtos = _customers.Select(c => new CustomerDto(c.Id.Value, c.Naam));
        return new ExampleSessionSnapshot(customerDtos);
    }

    public override void Load(ExampleSessionSnapshot snapshot)
    {
        _customers = snapshot.Customers.Select(dto => Customer.CreateExisting(new CustomerId() { Value = dto.Id }, dto.Naam));
        Expose<CustomerType, CustomerId>(_customers);
    }

    public override void Load(IReadOnlyDictionary<Type, object> entities)
    {
        _customers = ReadExposed<CustomerType, CustomerId>(entities).Values;
    }
}

public interface ICustomerRepository : IReadOnlyRepository<CustomerType, CustomerId>, IWriteOnlyRepository<CustomerType, CustomerId>
{
    public async Task<Result<CustomerType>> UpdateNaam(CustomerId Id, string newNaam)
    {
        return await GetByIdAsync(Id)
            .Map(customer => customer with { Naam = newNaam })
            .BindAsync(newCustomer => UpdateAsync(newCustomer));
    }
}

public class ExampleRepository(IDataStore store) : RepositoryBase<CustomerType, CustomerId>(store), ICustomerRepository;