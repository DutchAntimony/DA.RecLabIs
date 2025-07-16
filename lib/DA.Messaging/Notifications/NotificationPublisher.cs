using DA.Messaging.Notifications.Abstractions;
using DA.Results;
using DA.Results.Errors;
using DA.Results.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DA.Messaging.Notifications;

/// <summary>
/// Notification publisher that processes pending notifications and invokes their handlers.
/// It uses the <see cref="INotificationStore"/> to retrieve pending notifications and marks them as published.
/// The handlers are resolved from a scoped <see cref="IServiceProvider"/> based on the notification type.
/// </summary>
/// <param name="serviceScopeFactory">The IServiceScopeFactory to create a scope in which the handlers can be resolved.</param>
/// <param name="logger">To log the process</param>
internal sealed class NotificationPublisher(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<NotificationPublisher> logger) : INotificationPublisher
{
    // Caches the handler types, not the concrete implementation, for each notification type to avoid scanning multiple times.
    // The actual handler instances are resolved from the service provider when needed, with the correct scope.
    private readonly ConcurrentDictionary<Type, Type> _handlerInterfaceTypeCache = new();

    /// <inheritdoc />
    public async Task PublishAsync(CancellationToken cancellationToken = default)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var notificationStore = scope.ServiceProvider.GetRequiredService<INotificationStore>();
        var notifications = await notificationStore.GetPendingNotificationsAsync(cancellationToken);
        logger.LogDebug("{Service} has to publish {amount} notifications.", nameof(NotificationPublisher), notifications.Count);
        
        foreach (var notification in notifications)
        {
            var publishingResult = await PublishNotificationAsync(scope.ServiceProvider, notification, cancellationToken);
            await notificationStore.MarkAsPublishedAsync(notification.Id, publishingResult, cancellationToken);
        }
    }

    private async Task<NotificationProcessingResult> PublishNotificationAsync(IServiceProvider serviceProvider, INotification notification, CancellationToken cancellationToken)
    {
        var notificationType = notification.GetType();

        var handlerInterfaceType = _handlerInterfaceTypeCache.GetOrAdd(notificationType,
            t => typeof(INotificationHandler<>).MakeGenericType(t));

        var handlers = serviceProvider.GetServices(handlerInterfaceType).ToList();
        if (handlers.Count == 0)
        {
            logger.LogWarning("No handlers registered for {NotificationType}.", notificationType.Name);
            return NotificationProcessingResult.Failure(nameof(NotificationPublisher), "No handlers registered for this notification type.");
        }

        Result result = Result.Success();
        foreach (var handler in handlers)
        {
            result = await result.CheckAsync(() =>
                HandleNotificationForHandlerTypeAsync(serviceProvider, notification, handler!, handlerInterfaceType, cancellationToken));
        }

        return NotificationProcessingResult.FromResult(result, nameof(NotificationPublisher), DateTime.UtcNow);
    }

    private async Task<Result> HandleNotificationForHandlerTypeAsync(IServiceProvider serviceProvider, INotification notification, object handler, Type handlerInterfaceType, CancellationToken cancellationToken)
    {
        try
        {
            var method = handlerInterfaceType.GetMethod("HandleAsync")!;
            var task = (Task)method.Invoke(handler, [notification, cancellationToken])!;
            await task.ConfigureAwait(false);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing notification {NotificationType} with handler {HandlerType}.", notification.GetType().Name, handler.GetType().Name);
            return new UnexpectedError(ex);
        }
    }
}
