using DA.Messaging.Notifications.Abstractions;

namespace Messaging.Tests.Unit.Notifications.Samples;

public sealed class SampleNotificationHandler : INotificationHandler<SampleNotification>
{
    public static List<SampleNotification> HandledNotifications { get; } = [];

    public Task HandleAsync(SampleNotification notification, CancellationToken cancellationToken)
    {
        HandledNotifications.Add(notification);
        return Task.CompletedTask;
    }

    public static void Reset()
    {
        HandledNotifications.Clear();
    }
}
