using DA.Messaging.Notifications.Abstractions;
using System.Collections.Concurrent;

namespace DA.Messaging.Notifications;

/// <summary>
/// An implementation of <see cref="INotificationStore"/> that does stores all notifications in memory.
/// This is an example implementation and should probably not be used in production, 
/// because it does not persist notifications when the system goes down.
/// </summary>
internal sealed class InMemoryNotificationStore : INotificationStore
{
    private readonly ConcurrentDictionary<Guid, INotification> _notifications = [];

    /// <inheritdoc />
    public Task StoreAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        _notifications.TryAdd(notification.Id, notification);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IReadOnlyCollection<INotification>> GetPendingNotificationsAsync(CancellationToken cancellationToken = default)
    {
        var pendingNotifications = _notifications.Values
            .Where(n => !n.ProcessingResult.IsProcessed)
            .ToList();

        return Task.FromResult<IReadOnlyCollection<INotification>>(pendingNotifications.AsReadOnly());
    }

    /// <inheritdoc />
    public Task MarkAsPublishedAsync(Guid notificationId, NotificationProcessingResult processingResult, CancellationToken cancellationToken = default)
    {
        if (!_notifications.TryGetValue(notificationId, out var notification))
        {
            // If the notification is not known in the store, we cannot update it.
            throw new InvalidOperationException($"Notification with ID {notificationId} cannot be updated because it was not found.");
        }

        if (notification is not NotificationBase existing)
        {
            // If the notification is not a NotificationBase, we cannot update it.
            throw new InvalidOperationException($"Notification with ID {notificationId} does not derive from NotificationBase.");
        }

        var updated = existing.SetProcessed(processingResult);
        _notifications[notificationId] = updated;

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IReadOnlyCollection<INotification>> GetFailedSinceAsync(DateTime sinceUtc, CancellationToken cancellationToken = default)
    {
        var failedNotifications = _notifications.Values
            .Where(n => n.ProcessingResult.IsProcessed && !n.ProcessingResult.IsSuccessful)
            .Where(n => n.ProcessingResult.ProcessedAt >= sinceUtc)
            .ToList();

        return Task.FromResult<IReadOnlyCollection<INotification>>(failedNotifications.AsReadOnly());
    }
}