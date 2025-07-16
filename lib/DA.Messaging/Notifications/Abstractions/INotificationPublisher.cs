namespace DA.Messaging.Notifications.Abstractions;

/// <summary>
/// This is the publisher which can publish all notifications in the <see cref="INotificationStore"/>.
/// This interface can be injected into the background service that publishes notifications.
/// </summary>
public interface INotificationPublisher
{
    /// <summary>
    /// Publishes all stored notification to all registered handlers.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PublishAsync(CancellationToken cancellationToken = default);
}
