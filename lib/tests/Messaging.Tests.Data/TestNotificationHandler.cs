using DA.Messaging.Requests.Behaviours;

namespace Messaging.Tests.Data;

public sealed class TestNotificationHandler(ITestNotificationSink sink) 
    : INotificationHandler<RequestExceedsExpectedDurationNotification>
{
    public Task HandleAsync(RequestExceedsExpectedDurationNotification notification, CancellationToken cancellationToken)
    {
        sink.Add(notification);
        return Task.CompletedTask;
    }

}
