using DA.Foundations.Entities;
using DA.Foundations.Entities.Immutable;

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
        if (id.Value == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty.", nameof(id));
        return new CustomerType(id, naam);
    }
}