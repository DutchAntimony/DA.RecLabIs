# Feature Request: Declaratieve toegang tot exposed entities in SessionBase<T>

- Datum: 2025-07-20
- Status: New

## Probleem

Binnen het huidige model wordt het `SessionBase<TSessionSnapshot>` patroon gebruikt om domeinmodellen in-memory te beheren, en snapshots op te slaan via een `IEntityPersistenceStrategy`.

Het exposen van entiteitcollecties gebeurt via de `Expose<TEntity, TKey>()` methode, die de data opslaat in een interne `Dictionary<Type, object>`. Bij het herstellen van de sessie vanuit deze dictionary wordt momenteel gebruik gemaakt van een helpermethode `ReadExposed<TEntity, TKey>()`.

### Voorbeeldgebruik:

```csharp
_customers = ReadExposed<CustomerType, CustomerId>();
```

Hoewel dit correct werkt, zijn er enkele nadelen:
- De syntax is relatief laagdrempelig, maar niet intuïtief.
- De gebruiker moet de exacte types `TEntity` en `TKey` telkens meegeven.
- Het lekt interne implementatiedetails (`Dictionary<Type, object>`) naar gebruikerscode.
- Moeilijker uit te leggen of documenteren voor andere ontwikkelaars.

## Doel

Het doel is om een elegantere, typeveilige, intuïtieve manier te introduceren om exposed entiteiten op te vragen in concrete sessieklassen, <b>zonder gebruik van reflectie of duplicatie van type-informatie.</b> Dit moet leiden tot:
- Minder boilerplate
- Meer gebruikersgemak
- Betere IDE-ondersteuning (IntelliSense)
- Heldere scheiding tussen infrastructuur en domein

## Voorstel

### Source generator

Introduceer een source generator die voor een concrete `SessionBase<T>`-implementatie automatisch strongly typed properties genereert voor alle via `Expose<TEntity, TKey>()` opgeslagen entiteitcollecties.

Gebruiker schrijft:
``` csharp
[GenerateExposedAccessors]
public partial class ExampleSession : SessionBase<ExampleSnapshot>
{
    public override void Load(IReadOnlyDictionary<Type, object> entities)
    {
        _customers = Customers; // <-- gegenereerde property
    }

    public override ExampleSnapshot ToSnapshot() => ...;
    public override void Load(ExampleSnapshot snapshot) => ...;
}
```

Generator maakt aan:
``` csharp
// gegenereerd via partial class
public partial class ExampleSession
{
    public Dictionary<CustomerId, CustomerType> Customers =>
        ReadExposed<CustomerType, CustomerId>();

    public Dictionary<OrderId, Order> Orders =>
        ReadExposed<Order, OrderId>();
}
```

De mapping van types naar properties kan expliciet gemaakt worden via attributen, of afgeleid worden uit de `Expose()` aanroepen (mits analyseerbaar).

### Implementatie details

- De source generator scant concrete `SessionBase<T>`-types.
- Als de sessie gemarkeerd is met `[GenerateExposedAccessors]`, zoekt de generator naar expose-aanroepen of accepteert handmatige registratie via attributen.
- Genereert een partial class met `Dictionary<TKey, TEntity>`-properties die de onderliggende `ReadExposed` aanroepen.

## Conclusie

De `Expose`/`ReadExposed` aanpak functioneert technisch goed, maar mist elegantie en eenvoud in gebruik. Een source generator kan dit probleem oplossen zonder impact op performance of complexiteit van de infrastructuur. Dit sluit goed aan bij het principe van expliciete controle in het domein, terwijl het infrastructuurniveau geoptimaliseerd wordt voor gebruiksgemak.

Een proof-of-concept generator zou relatief eenvoudig te implementeren zijn en kan in een latere fase worden opgenomen in de library of als opt-in NuGet package.
