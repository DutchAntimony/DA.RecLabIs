namespace Messaging.Tests.Data;

public sealed record SampleNotification(string Payload) : NotificationBase("Messaging.Tests.Unit");
