using DA.Messaging.Requests.Behaviours;
using Messaging.Tests.Unit.Notifications.Samples;
using Messaging.Tests.Unit.Requests.Samples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Tests.Unit.Notifications;
public class NotificationIntegrationTests
{

    [Fact]
    public async Task Should_Publish_Notification_And_Invoke_Handler()
    {
        var services = new ServiceCollection();
        services.AddNotificationMessaging(options => options.FromAssemblyContaining<SampleNotificationHandler>());

        var sink = new InMemoryTestNoficationSink();
        services.AddSingleton<ITestNotificationSink>(sink);

        var provider = services.BuildServiceProvider();

        var payload = "TestPayload";

        var store = provider.GetRequiredService<INotificationStore>();
        var publisher = provider.GetRequiredService<INotificationPublisher>();

        var notification = new SampleNotification(payload);
        
        await store.StoreAsync(notification, CancellationToken.None);

        var pending = await store.GetPendingNotificationsAsync(CancellationToken.None);
        pending.Count.ShouldBe(1);

        await publisher.PublishAsync(CancellationToken.None);

        sink.Notifications.Count.ShouldBe(1);

        pending = await store.GetPendingNotificationsAsync(CancellationToken.None);
        pending.ShouldBeEmpty();

        var failed = await store.GetFailedSinceAsync(DateTime.MinValue, CancellationToken.None);
        failed.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_MarkNotificationAsUnhandler_IfNoHandlerIsAvailable()
    {
        var services = new ServiceCollection();
        services.AddNotificationMessaging(); // No handlers registered, so it should not find any handlers for SampleNotification

        var sink = new InMemoryTestNoficationSink();
        services.AddSingleton<ITestNotificationSink>(sink);

        var provider = services.BuildServiceProvider();

        var payload = "TestPayload";

        var store = provider.GetRequiredService<INotificationStore>();
        var publisher = provider.GetRequiredService<INotificationPublisher>();

        var notification = new SampleNotification(payload);

        await store.StoreAsync(notification, CancellationToken.None);

        var pending = await store.GetPendingNotificationsAsync(CancellationToken.None);
        pending.Count.ShouldBe(1);

        await publisher.PublishAsync(CancellationToken.None);

        sink.Notifications.Count.ShouldBe(0);

        pending = await store.GetPendingNotificationsAsync(CancellationToken.None);
        pending.ShouldBeEmpty();

        var failedNotifications = await store.GetFailedSinceAsync(DateTime.MinValue, CancellationToken.None);

        var actualFailed = failedNotifications.SingleOrDefault();
        actualFailed.ShouldNotBeNull();
        actualFailed.Id.ShouldBe(notification.Id);

        actualFailed.ProcessingResult.ShouldNotBeNull();
        actualFailed.ProcessingResult.IsProcessed.ShouldBeTrue();
        actualFailed.ProcessingResult.IsSuccessful.ShouldBeFalse();
        actualFailed.ProcessingResult.ProcessedBy.ShouldBe("NotificationPublisher");
        actualFailed.ProcessingResult.ProcessedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        actualFailed.ProcessingResult.ErrorMessage.ShouldBe("No handlers registered for this notification type.");
    }

    [Fact]
    public async Task Pipeline_WithPerformanceLogging_EmitsNotification_WhenRequestIsSlow()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder().Build();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddRequestMessaging(options =>
        {
            options.FromAssemblyContaining<SlowSampleQueryHandler>();
            options.AddPerformanceLoggingBehavior(configure =>
            {
                configure.MaxExpectedDuration = TimeSpan.FromMilliseconds(50);
            });
        });

        services.AddNotificationMessaging(options => options.FromAssemblyContaining<TestNotificationHandler>());

        var sink = new InMemoryTestNoficationSink();
        services.AddSingleton<ITestNotificationSink>(sink);

        var provider = services.BuildServiceProvider();

        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();
        var publisher = provider.GetRequiredService<INotificationPublisher>();
        var store = provider.GetRequiredService<INotificationStore>();

        var result = await dispatcher.DispatchAsync(new SlowSampleQuery(TimeSpan.FromMilliseconds(75)), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.TryGetValue(out var value).ShouldBeTrue();
        value.ShouldBe("response from slow handler");

        // op dit moment zou de notification moeten zijn opgeslagen in de store
        var pendingNotifications = await store.GetPendingNotificationsAsync(CancellationToken.None);
        pendingNotifications.Count.ShouldBe(1);

        // nu publiceren we de notification
        await publisher.PublishAsync(CancellationToken.None);

        // en nu zou de notification handler deze moeten hebben verwerkt
        sink.Notifications.Count.ShouldBe(1);
        var notification = sink.Notifications.Single() as RequestExceedsExpectedDurationNotification;
        notification.ShouldNotBeNull();
        notification.RequestType.ShouldBe(nameof(SlowSampleQuery));
        notification.ResponseType.ShouldBe("Result<String>");
        notification.Duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(75));
    }
}
