namespace Messaging.Tests.Unit.Notifications.Samples;

public sealed class SampleNotificationHandler(ITestNotificationSink sink)
    : INotificationHandler<SampleNotification>
{
    public Task HandleAsync(SampleNotification notification, CancellationToken cancellationToken)
    {
        sink.Add(notification);
        return Task.CompletedTask;
    }
}