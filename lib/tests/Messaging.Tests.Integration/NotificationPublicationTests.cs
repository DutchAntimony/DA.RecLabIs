using Messaging.Tests.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Tests.Integration;

public class NotificationPublicationTests
{
    [Fact]
    public async Task PublishedNotification_ShouldGetHandled()
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

}
