namespace Messaging.Tests.Data;

public sealed record SampleNotificationCustomCreatingTime(string Payload, DateTime CreatedAt) : NotificationBase("Messaging.Tests.Unit", CreatedAt);