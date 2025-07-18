using DA.Foundations.Entities;
using DA.Optional;
using DA.Optional.Extensions;
using DA.Results;
using DA.Results.Errors;
using DA.Results.Extensions;
using System.Linq.Expressions;

namespace DA.Foundations.DataAccess;

/// <summary>
/// Generic abstract base repository that implements both the <see cref="IReadOnlyRepository{TEntity, TKey}"/>
/// as well as the <see cref="IWriteOnlyRepository{TEntity, TKey}"/>
/// </summary>
/// <remarks>This base should be used to create a specific repository for a TEntity without generic type arguments.
/// The ReadOnly and WriteOnly interfaces can be used to create two specific repository interfaces for the actual entity.
/// It is recommended to use one actual implementation that is both the implementation for the ReadOnly as well as the WriteOnly contract.</remarks>
/// <typeparam name="TEntity">The type of the entity that is in this repository.</typeparam>
/// <typeparam name="TKey">The type of the strongly typed key for this repository.</typeparam>
/// <param name="dataStore">Store that contains the actual data. This can be coming from EF Core, from a json data import, from in memory, etc.</param>
public abstract class RepositoryBase<TEntity, TKey>(IDataStore dataStore) : IReadOnlyRepository<TEntity, TKey>, IWriteOnlyRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : struct, IEntityKey
{
    /// <summary>
    /// The datastore that contains the actual data.
    /// </summary>
    /// <remarks>The datastore is exposed to make more optimized calls to the store possible for overrides of the repository base.
    /// The underlying store should not be exposed to the application layer directly.
    /// </remarks>
    protected readonly IDataStore DataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var queryable = await DataStore.QueryAsync<TEntity, TKey>(cancellationToken);
        return queryable.AsEnumerable();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var queryable = await DataStore.QueryAsync<TEntity, TKey>(cancellationToken);
        return queryable.Where(predicate).AsEnumerable();
    }

    /// <inheritdoc />
    public async Task<Result<TEntity>> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entityOption = await DataStore.FindByIdAsync<TEntity, TKey>(id, cancellationToken);
        return entityOption.Match(
            entity => Result.Success(entity),
            () => new NotFoundError(typeof(TEntity).Name, $"By Id '{id.Value}'"));
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entityOption = await DataStore.FindByIdAsync<TEntity, TKey>(id, cancellationToken);
        return entityOption.HasValue;
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var queryable = await DataStore.QueryAsync<TEntity, TKey>(cancellationToken);
        return queryable.Count();
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var queryable = await DataStore.QueryAsync<TEntity, TKey>(cancellationToken);
        return queryable.Count(predicate);
    }

    /// <inheritdoc />
    public async Task<Result<TEntity>> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var queryable = await DataStore.QueryAsync<TEntity, TKey>(cancellationToken);
        try
        {
            var entity = queryable.SingleOrDefault(predicate);
            if (entity == null)
            {
                return new NotFoundError(typeof(TEntity).Name, "By specified predicate");
            }
            return Result.Success(entity);
        }
        catch (InvalidOperationException ex)
        {
            return new UnexpectedError(ex, "More than one entity found for the specified predicate.");
        }
    }

    /// <inheritdoc />
    public async Task<Option<TEntity>> GetSingleOrNoneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var queryable = await DataStore.QueryAsync<TEntity, TKey>(cancellationToken);
        var entity = queryable.SingleOrDefault(predicate);
        return entity.AsOption();
    }

    /// <inheritdoc />
    public async Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return await Result.Success(entity)
            .CheckNotAsync(entity => ExistsAsync(entity.Id), new DomainError($"Can't insert {typeof(TEntity).Name} with Id {entity.Id.Value} because entity already exisits."))
            .TapAsync(entity => DataStore.AddOrUpdateAsync<TEntity, TKey>(entity, cancellationToken));
    }

    /// <inheritdoc />
    public async Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return await Result.Success(entity)
            .CheckAsync(entity => ExistsAsync(entity.Id), new NotFoundError(typeof(TEntity).Name, $"By Id: {entity.Id.Value}"))
            .TapAsync(entity => DataStore.AddOrUpdateAsync<TEntity, TKey>(entity, cancellationToken));
    }

    /// <inheritdoc />
    public async Task<Result> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DataStore.RemoveAsync<TEntity, TKey>(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return await DataStore.RemoveAsync<TEntity, TKey>(entity.Id, cancellationToken);
    }

}
