using DA.Messaging.Notifications;
using Messaging.Tests.Unit.Notifications.Samples;

namespace Messaging.Tests.Unit.Notifications;

public class NotificationBaseTests
{
    [Fact]
    public void Constructor_Should_CreateNotificationWithDefaultValues()
    {
        var notification = new SampleNotification("Payload");
        notification.Id.ShouldNotBe(Guid.Empty);
        notification.CreatedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        notification.Sender.ShouldBe("Messaging.Tests.Unit");
        notification.Type.ShouldBe(nameof(SampleNotification));
        notification.ProcessingResult.ShouldBe(NotificationProcessingResult.NotProcessed);
    }

    [Fact]
    public void Constructor_Should_CreateWithCustomCreatedAt()
    {
        var customCreatedAt = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var notification = new SampleNotificationCustomCreatingTime("Payload", customCreatedAt);
        
        notification.Id.ShouldNotBe(Guid.Empty);
        notification.CreatedAt.ShouldBe(customCreatedAt);
        notification.Sender.ShouldBe("Messaging.Tests.Unit");
        notification.Type.ShouldBe(nameof(SampleNotificationCustomCreatingTime));
        notification.ProcessingResult.ShouldBe(NotificationProcessingResult.NotProcessed);
    }

    [Fact]
    public void SetProcessed_Should_UpdatesProcessingResult()
    {
        var notification = new SampleNotification("TestSender");
        var processingResult = NotificationProcessingResult.Success("Processor");

        var updatedNotification = notification.SetProcessed(processingResult);
        
        updatedNotification.ProcessingResult.ShouldBe(processingResult);
    }
}
