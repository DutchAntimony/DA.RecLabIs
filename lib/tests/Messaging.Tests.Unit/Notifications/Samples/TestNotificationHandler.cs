using DA.Messaging.Requests.Behaviours;

namespace Messaging.Tests.Unit.Notifications.Samples;

public sealed class TestNotificationHandler(ITestNotificationSink sink) 
    : INotificationHandler<RequestExceedsExpectedDurationNotification>
{
    public Task HandleAsync(RequestExceedsExpectedDurationNotification notification, CancellationToken cancellationToken)
    {
        sink.Add(notification);
        return Task.CompletedTask;
    }

}

public interface ITestNotificationSink
{
    void Add(INotification notification);
    IReadOnlyList<INotification> Notifications { get; }
}

public class InMemoryTestNoficationSink : ITestNotificationSink
{
    private readonly List<INotification> _notifications = [];

    public void Add(INotification notification) => _notifications.Add(notification);

    public IReadOnlyList<INotification> Notifications => _notifications.AsReadOnly();
}