using DA.Foundations.DataAccess;
using DA.Foundations.DataAccess.Persistence;
using DA.Foundations.DependencyInjection;
using DA.Foundations.Examples;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Foundations.Tests.Integration;
public class DependencyInjectionTests
{
    [Fact]
    public void UseSessions_Should_AddSessionStoreAndPersistenceStrategy()
    {
        var services = new ServiceCollection();

        services.UseSession<ExampleSession, ExampleSessionSnapshot>();

        var serviceProvider = services.BuildServiceProvider();
        
        var fileDataStore = serviceProvider.GetService<IFileDataStore>();
        fileDataStore.ShouldNotBeNull();
        var dataStore = serviceProvider.GetService<IDataStore>();
        dataStore.ShouldNotBeNull();
        fileDataStore.Equals(dataStore).ShouldBeTrue();

        var persistenceStrategy = serviceProvider.GetService<ISnapshotPersistenceStrategy<ExampleSessionSnapshot>>();
        persistenceStrategy.ShouldNotBeNull();
        persistenceStrategy.ShouldBeOfType<JsonFilePersistenceStrategy<ExampleSessionSnapshot>>();
    }

    [Fact]
    public void UseSessions_ShouldThrow_WhenPersistenceStrategyCannotBeResolved()
    {
        var services = new ServiceCollection();

        services.UseSession<ExampleSession, ExampleSessionSnapshot>(options =>
        {
            options.Mode = PersistenceMode.EncryptedJsonFile;
            options.EncryptionKey = "VerySecureKey";
        });

        var serviceProvider = services.BuildServiceProvider();

        Should.Throw<NotSupportedException>(() => 
            serviceProvider.GetService<ISnapshotPersistenceStrategy<ExampleSessionSnapshot>>());
    }

    [Fact]
    public void UseSessions_Should_BeConfigurableWithCustomPersistenceStrategy()
    {
        var services = new ServiceCollection();

        services.UseSession<ExampleSession, ExampleSessionSnapshot>((sessionProvider) =>
            new JsonFilePersistenceStrategy<ExampleSessionSnapshot>());

        var serviceProvider = services.BuildServiceProvider();

        var fileDataStore = serviceProvider.GetService<IFileDataStore>();
        fileDataStore.ShouldNotBeNull();
        var dataStore = serviceProvider.GetService<IDataStore>();
        dataStore.ShouldNotBeNull();
        fileDataStore.Equals(dataStore).ShouldBeTrue();

        var persistenceStrategy = serviceProvider.GetService<ISnapshotPersistenceStrategy<ExampleSessionSnapshot>>();
        persistenceStrategy.ShouldNotBeNull();
        persistenceStrategy.ShouldBeOfType<JsonFilePersistenceStrategy<ExampleSessionSnapshot>>();
    }
}
