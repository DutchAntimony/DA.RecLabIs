namespace Messaging.Tests.Unit.Notifications.Samples;

public sealed record SampleNotification(string Payload) : NotificationBase("Messaging.Tests.Unit");

public sealed record SampleNotificationCustomCreatingTime(string Payload, DateTime CreatedAt) : NotificationBase("Messaging.Tests.Unit", CreatedAt);