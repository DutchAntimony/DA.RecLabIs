namespace DA.Messaging.Notifications.Abstractions;

/// <summary>
/// Abstract base class for notifications that can be sent through the messaging system.
/// Makes sure that all notifications have a unique identifier and a creation timestamp.
/// </summary>
public abstract record NotificationBase : INotification
{
    public NotificationBase(string sender, DateTime? created = null) =>
        (Sender, CreatedAt) = (sender, created ?? DateTime.UtcNow);

    /// <inheritdoc />
    public Guid Id { get; init; } = Guid.CreateVersion7();
    
    /// <inheritdoc />
    public DateTime CreatedAt { get; init; }

    /// <inheritdoc />
    public string Sender { get; init; }

    /// <summary>
    /// For serialization purposes, this property returns the type name of the notification.
    /// </summary>
    public string Type => GetType().Name;

    /// <summary>
    /// <inheritdoc />
    /// Starts as <see cref="NotificationProcessingResult.NotProcessed"/> and can be updated to indicate the processing status.
    /// </summary>
    public NotificationProcessingResult ProcessingResult { get; init; } = NotificationProcessingResult.NotProcessed;

    /// <summary>
    /// Create a copy of the notification with the specified processing result.
    /// </summary>
    /// <param name="processingResult">The result of the processing.</param>
    public NotificationBase SetProcessed(NotificationProcessingResult processingResult)
    {
        return this with { ProcessingResult = processingResult };
    }
}
