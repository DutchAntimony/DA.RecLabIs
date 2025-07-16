namespace DA.Messaging.Notifications;

/// <summary>
/// Interface for notifications that can be sent through the messaging system.
/// </summary>
public interface INotification
{
    /// <summary>
    /// Unique identifier for the notification.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Date and time when the notification was created.
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// The sender of the notification. 
    /// This is typically the name of the service or component that created the notification.
    /// </summary>
    string Sender { get; init; }

    /// <summary>
    /// Result of processing the notification.
    /// </summary>
    NotificationProcessingResult ProcessingResult { get; init; }
}
