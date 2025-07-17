using Messaging.Tests.Unit.Notifications.Samples;
using Messaging.Tests.Unit.Requests.Samples;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Messaging.Tests.Unit.DependencyInjection;

public class DependencyInjectionExtensionsTests
{
    [Fact]
    public void AddRequestMessaging_ShouldAddRequestDispatcher()
    {
        var services = new ServiceCollection();

        services.AddRequestMessaging();

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<IRequestDispatcher>().ShouldNotBeNull();
    }

    [Fact]
    public void AddRequestMessaging_Options_FromAssembly_ShouldAddHandlers()
    {
        var services = new ServiceCollection();

        services.AddRequestMessaging(options => options.FromAssembly(typeof(SampleQuery).Assembly));

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<IRequestDispatcher>().ShouldNotBeNull();
        serviceProvider.GetService<IRequestHandler<SampleQuery, Result<string>>>().ShouldNotBeNull();
    }

    [Fact]
    public void AddRequestMessaging_Options_FromAssemblies_ShouldAddHandlers()
    {
        var services = new ServiceCollection();

        services.AddRequestMessaging(options => options.FromAssemblies(typeof(SampleQuery).Assembly));

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<IRequestDispatcher>().ShouldNotBeNull();
        serviceProvider.GetService<IRequestHandler<SampleQuery, Result<string>>>().ShouldNotBeNull();
    }

    [Fact]
    public void AddRequestMessaging_Options_FromAssemblyContaining_ShouldAddHandlers()
    {
        var services = new ServiceCollection();

        services.AddRequestMessaging(options => options.FromAssemblyContaining<SampleQuery>());

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<IRequestDispatcher>().ShouldNotBeNull();
        serviceProvider.GetService<IRequestHandler<SampleQuery, Result<string>>>().ShouldNotBeNull();
    }

    [Fact]
    public void AddNotificationMessaging_ShouldAddNotificationPublisherAndStore()
    {
        var services = new ServiceCollection();

        services.AddNotificationMessaging();

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<INotificationPublisher>().ShouldNotBeNull();
        serviceProvider.GetService<INotificationStore>().ShouldNotBeNull();
    }

    [Fact]
    public void AddNotificationMessaging_Options_FromAssembly_ShouldAddHandlers()
    {
        var services = new ServiceCollection();

        services.AddNotificationMessaging(options => options.FromAssembly(typeof(SampleNotification).Assembly));
        var sink = new InMemoryTestNoficationSink();
        services.AddSingleton<ITestNotificationSink>(sink);

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<INotificationPublisher>().ShouldNotBeNull();
        serviceProvider.GetServices<INotificationHandler<SampleNotification>>().ShouldNotBeEmpty();
    }

    [Fact]
    public void AddNotificationMessaging_Options_FromAssemblies_ShouldAddHandlers()
    {
        var services = new ServiceCollection();

        services.AddNotificationMessaging(options => options.FromAssemblies(typeof(SampleNotification).Assembly));
        var sink = new InMemoryTestNoficationSink();
        services.AddSingleton<ITestNotificationSink>(sink);

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<INotificationPublisher>().ShouldNotBeNull();
        serviceProvider.GetServices<INotificationHandler<SampleNotification>>().ShouldNotBeEmpty();
    }

    [Fact]
    public void AddNotificationMessaging_Options_FromAssemblyContaining_ShouldAddHandlers()
    {
        var services = new ServiceCollection();

        services.AddNotificationMessaging(options => options.FromAssemblyContaining<SampleNotification>());
        var sink = new InMemoryTestNoficationSink();
        services.AddSingleton<ITestNotificationSink>(sink);

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<INotificationPublisher>().ShouldNotBeNull();
        serviceProvider.GetServices<INotificationHandler<SampleNotification>>().ShouldNotBeEmpty();
    }

    [Fact]
    public void AddNotificationMessaging_Options_UseCustomNotificationStore_ShouldAddCustomNotificationStore()
    {
        var services = new ServiceCollection();

        services.AddNotificationMessaging(options => options.UseCustomNotificationStore<SampleNotificationStore>());

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<INotificationPublisher>().ShouldNotBeNull();
        serviceProvider.GetService<INotificationStore>().ShouldNotBeNull();
    }

    [ExcludeFromCodeCoverage]
    private sealed class SampleNotificationStore : INotificationStore
    {
        public async Task StoreAsync(INotification notification, CancellationToken cancellationToken)
        {
            await Task.Yield();
        }

        public async Task<IReadOnlyCollection<INotification>> GetPendingNotificationsAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            return [];
        }

        public async Task MarkAsPublishedAsync(Guid notificationId, NotificationProcessingResult processingResult, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
        }

        public async Task<IReadOnlyCollection<INotification>> GetFailedSinceAsync(DateTime sinceUtc, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return [];
        }
    }
}
