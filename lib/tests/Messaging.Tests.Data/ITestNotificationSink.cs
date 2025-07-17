namespace Messaging.Tests.Data;

public interface ITestNotificationSink
{
    void Add(INotification notification);
    IReadOnlyList<INotification> Notifications { get; }
}
