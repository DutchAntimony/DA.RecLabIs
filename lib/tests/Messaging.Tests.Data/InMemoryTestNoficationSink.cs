namespace Messaging.Tests.Data;

public class InMemoryTestNoficationSink : ITestNotificationSink
{
    private readonly List<INotification> _notifications = [];

    public void Add(INotification notification) => _notifications.Add(notification);

    public IReadOnlyList<INotification> Notifications => _notifications.AsReadOnly();
}