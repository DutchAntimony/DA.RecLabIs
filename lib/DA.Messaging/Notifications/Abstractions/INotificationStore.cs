namespace DA.Messaging.Notifications.Abstractions;

/// <summary>
/// Store for notifications that can be published by the <see cref="INotificationPublisher"/>.
/// This is the outbox contract that can be used to store notifications before they are published.
/// </summary>
public interface INotificationStore
{
    /// <summary>
    /// Stores a notification processing result in the outbox.
    /// </summary>
    /// <param name="notification">The notification to store.</param>
    Task StoreAsync(INotification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all pending notifications that have not been published yet.
    /// </summary>
    Task<IReadOnlyCollection<INotification>> GetPendingNotificationsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a notification as published with the specified processing result.
    /// </summary>
    /// <param name="notificationId">The identifier of het notication to mark as processed.</param>
    /// <param name="processingResult">The result of processing the notification.</param>
    Task MarkAsPublishedAsync(Guid notificationId, NotificationProcessingResult processingResult, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all notifications that have failed processing since the specified date and time.
    /// </summary>
    /// <remarks>This can be used for retry mechanisms or for diagnostic reasons.</remarks>
    /// <param name="sinceUtc">Give all the messages since this given time.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A readonly collection of <see cref="INotification"> because modifications are not supported.</returns>
    Task<IReadOnlyCollection<INotification>> GetFailedSinceAsync(DateTime sinceUtc, CancellationToken cancellationToken = default);
}