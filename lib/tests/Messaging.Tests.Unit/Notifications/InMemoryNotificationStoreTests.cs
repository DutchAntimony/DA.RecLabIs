using Messaging.Tests.Data;

namespace Messaging.Tests.Unit.Notifications;

public class InMemoryNotificationStoreTests
{
    private readonly INotificationStore _store = new InMemoryNotificationStore();
    private readonly SampleNotification _notification = new SampleNotification("TestPayload");

    [Fact]
    public async Task StoreAsync_Should_AddNotificationToStore()
    {
        await _store.StoreAsync(_notification, CancellationToken.None);

        var pendingNotifications = await _store.GetPendingNotificationsAsync(CancellationToken.None);
        pendingNotifications.ShouldContain(_notification);
    }

    [Fact]
    public async Task StoreAsync_Should_DoNothing_WhenNotificationWithThisIdIsAlreadyAdded()
    {
        await _store.StoreAsync(_notification, CancellationToken.None);
        await _store.StoreAsync(_notification, CancellationToken.None); // Attempt to add the same notification again
        var pendingNotifications = await _store.GetPendingNotificationsAsync(CancellationToken.None);
        pendingNotifications.Count.ShouldBe(1); // Should still contain only one instance
    }

    [Fact]
    public async Task GetPendingNotificationsAsync_Should_ReturnOnlyPendingNotifications()
    {
        var processedNotification = _notification.SetProcessed(NotificationProcessingResult.Success("test"));

        await _store.StoreAsync(_notification, CancellationToken.None);
        await _store.StoreAsync(processedNotification, CancellationToken.None);
        var pendingNotifications = await _store.GetPendingNotificationsAsync(CancellationToken.None);
        pendingNotifications.Count.ShouldBe(1);
        pendingNotifications.ShouldContain(_notification);
    }

    [Fact]
    public async Task MarkAsPublishedAsync_Should_UpdateNotificationProcessingResult()
    {
        await _store.StoreAsync(_notification, CancellationToken.None);
        var processingResult = NotificationProcessingResult.Success("Processor");
        await _store.MarkAsPublishedAsync(_notification.Id, processingResult, CancellationToken.None);
        
        var pendingNotifications = await _store.GetPendingNotificationsAsync(CancellationToken.None);
        pendingNotifications.ShouldBeEmpty();    
    }

    [Fact]
    public async Task MarkAsPublishedAsync_Should_ThrowException_WhenNotificationNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var processingResult = NotificationProcessingResult.Success("Processor");
        await Should.ThrowAsync<InvalidOperationException>(
            () => _store.MarkAsPublishedAsync(nonExistentId, processingResult, CancellationToken.None));
    }

    [Fact]
    public async Task MarkAsPublishedAsync_Should_ThrowException_WhenNotificationIsNotNotificationBase()
    {
        var notification = Substitute.For<INotification>();
        var processingResult = NotificationProcessingResult.Success("Processor");
        await _store.StoreAsync(notification, CancellationToken.None);
        await Should.ThrowAsync<InvalidOperationException>(
                    () => _store.MarkAsPublishedAsync(notification.Id, processingResult, CancellationToken.None));
    }

    [Fact]
    public async Task GetFailedSinceAsync_Should_ReturnFailedNotificationsSinceGivenDate()
    {
        var failedNotification = _notification.SetProcessed(NotificationProcessingResult.Failure("Processor", "Error message"));
        await _store.StoreAsync(failedNotification, CancellationToken.None);
        var sinceDate = DateTime.UtcNow.AddMinutes(-1);
        var failedNotifications = await _store.GetFailedSinceAsync(sinceDate, CancellationToken.None);
        
        failedNotifications.ShouldContain(failedNotification);
    }

    [Fact]
    public async Task GetFailedSinceAsync_Should_FilterOutOldNotifications()
    {
        var failedNotification = _notification.SetProcessed(NotificationProcessingResult.Failure("Processor", "Error message", new DateTime(2023, 1, 1)));
        await _store.StoreAsync(failedNotification, CancellationToken.None);
        var sinceDate = new DateTime(2024,1,1);// after the failed notification was created
        var failedNotifications = await _store.GetFailedSinceAsync(sinceDate, CancellationToken.None);

        failedNotifications.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetFailedSinceAsync_Should_FilterOutUnprocessedNotifications()
    {
        await _store.StoreAsync(_notification, CancellationToken.None);
        var sinceDate = new DateTime(2024, 1, 1);// before the notification was created
        var failedNotifications = await _store.GetFailedSinceAsync(sinceDate, CancellationToken.None);

        failedNotifications.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetFailedSinceAsync_Should_FilterOutSuccessfullyProcessedNotifications()
    {
        var successfulNotification = _notification.SetProcessed(NotificationProcessingResult.Success("Processor"));
        await _store.StoreAsync(successfulNotification, CancellationToken.None);
        var sinceDate = new DateTime(2024, 1, 1);// before the notification was created
        var failedNotifications = await _store.GetFailedSinceAsync(sinceDate, CancellationToken.None);

        failedNotifications.ShouldBeEmpty();
    }
}