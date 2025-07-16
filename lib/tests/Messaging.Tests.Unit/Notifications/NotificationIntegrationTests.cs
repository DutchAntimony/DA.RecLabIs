using Microsoft.Extensions.DependencyInjection;
using Messaging.Tests.Unit.Notifications.Samples;

namespace Messaging.Tests.Unit.Notifications;
public class NotificationIntegrationTests
{
    public NotificationIntegrationTests()
    {

    }

    [Fact]
    public async Task Should_Publish_Notification_And_Invoke_Handler()
    {
        SampleNotificationHandler.Reset();
        var services = new ServiceCollection();
        services.AddNotificationMessaging(options => options.FromAssemblyContaining<SampleNotificationHandler>());
        var provider = services.BuildServiceProvider();

        var payload = "TestPayload";

        var store = provider.GetRequiredService<INotificationStore>();
        var publisher = provider.GetRequiredService<INotificationPublisher>();

        var notification = new SampleNotification(payload);
        
        await store.StoreAsync(notification, CancellationToken.None);

        var pending = await store.GetPendingNotificationsAsync(CancellationToken.None);
        pending.Count.ShouldBe(1);

        await publisher.PublishAsync(CancellationToken.None);
        
        SampleNotificationHandler.HandledNotifications.Count.ShouldBe(1);
        SampleNotificationHandler.HandledNotifications[0].Payload.ShouldBe(payload);

        pending = await store.GetPendingNotificationsAsync(CancellationToken.None);
        pending.ShouldBeEmpty();

        var failed = await store.GetFailedSinceAsync(DateTime.MinValue, CancellationToken.None);
        failed.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_MarkNotificationAsUnhandler_IfNoHandlerIsAvailable()
    {
        SampleNotificationHandler.Reset();
        var services = new ServiceCollection();
        services.AddNotificationMessaging(); // No handlers registered, so it should not find any handlers for SampleNotification
        var provider = services.BuildServiceProvider();

        var payload = "TestPayload";

        var store = provider.GetRequiredService<INotificationStore>();
        var publisher = provider.GetRequiredService<INotificationPublisher>();

        var notification = new SampleNotification(payload);

        await store.StoreAsync(notification, CancellationToken.None);

        var pending = await store.GetPendingNotificationsAsync(CancellationToken.None);
        pending.Count.ShouldBe(1);

        await publisher.PublishAsync(CancellationToken.None);

        SampleNotificationHandler.HandledNotifications.Count.ShouldBe(0);

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
