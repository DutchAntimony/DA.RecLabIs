namespace Messaging.Tests.Data;

public sealed class SampleNotificationHandler(ITestNotificationSink sink)
    : INotificationHandler<SampleNotification>
{
    public Task HandleAsync(SampleNotification notification, CancellationToken cancellationToken)
    {
        sink.Add(notification);
        return Task.CompletedTask;
    }
}