using DA.Foundations.DataAccess;
using DA.Foundations.DataAccess.Persistence;
using DA.Foundations.DataAccess.Sessions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DA.Foundations.DependencyInjection;

/// <summary>
/// Dependency Injections methods for the DA.Foundations library
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Use session based entity mapping using a Session and a SessionSnapshot.
    /// </summary>
    /// <typeparam name="TSession">The implementation of <see cref="SessionBase{TSnapshot}"/> to use.</typeparam>
    /// <typeparam name="TSnapshot">The implementation of the snapshot.</typeparam>
    /// <param name="configure">Action to configure the options for the session based entity mapping.</param>
    /// <returns>The <see cref="IServiceCollection"/> to chain configuration.</returns>
    public static IServiceCollection UseSession<TSession, TSnapshot>(this IServiceCollection services, Action<SessionOptions>? configure = null)
            where TSession : SessionBase<TSnapshot>, new() where TSnapshot : class
    {
        services.Configure(configure ?? (_ => { }));

        services.AddSingleton<ISnapshotPersistenceStrategy<TSnapshot>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<SessionOptions>>().Value;

            return options.Mode switch
            {
                PersistenceMode.JsonFile =>
                    new JsonFilePersistenceStrategy<TSnapshot>(),

                //PersistenceMode.EncryptedJsonFile =>
                //    new EncryptedJsonFilePersistenceStrategy<TSnapshot>(
                //        options.EncryptionKey ?? throw new InvalidOperationException("EncryptionKey must be provided.")),

                _ => throw new NotSupportedException($"Persistence mode '{options.Mode}' is not (yet) supported.")
            };
        });

        services.AddSingleton<IFileDataStore, SessionDataStore<TSession, TSnapshot>>();
        services.AddSingleton<IDataStore>(sp => sp.GetRequiredService<IFileDataStore>());

        return services;
    }

    /// <summary>
    /// Use session based entity mapping using a Session and a SessionSnapshot.
    /// </summary>
    /// <typeparam name="TSession">The implementation of <see cref="SessionBase{TSnapshot}"/> to use.</typeparam>
    /// <typeparam name="TSnapshot">The implementation of the snapshot.</typeparam>
    /// <param name="persistenceStrategyFactory">Function to use a custom persistence strategy</param>
    /// <returns>The <see cref="IServiceCollection"/> to chain configuration.</returns>
    public static IServiceCollection UseSession<TSession, TSnapshot>(
        this IServiceCollection services,
        Func<IServiceProvider, ISnapshotPersistenceStrategy<TSnapshot>> persistenceStrategyFactory)
        where TSession : SessionBase<TSnapshot>, new()
    {
        services.AddSingleton(persistenceStrategyFactory);
        services.AddSingleton<IFileDataStore, SessionDataStore<TSession, TSnapshot>>();
        services.AddSingleton<IDataStore>(sp => sp.GetRequiredService<IFileDataStore>());

        return services;
    }
}


