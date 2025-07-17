using DA.Messaging.Extensions;
using DA.Messaging.Notifications.Abstractions;
using DA.Messaging.Requests.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DA.Messaging.Requests.Behaviours;

/// <summary>
/// Logging behaviour that logs the performance of handling a request.
/// It will raise a <see cref="RequestExceedsExpectedDurationNotification"/> notification is the request takes longer than expected.
/// </summary>
/// <typeparam name="TRequest">The type of the request</typeparam>
/// <typeparam name="TResponse">The type of the response that matches the request</typeparam>
/// <param name="logger">The logger to write the log messages to.</param>
public sealed class PerformanceLoggingBehaviour<TRequest, TResponse>(
    IOptions<PerformanceLoggingOptions> options,
    INotificationStore notificationStore,
    ILogger<PerformanceLoggingBehaviour<TRequest, TResponse>> logger) 
    : IRequestPipelineBehaviour<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        var requestName = request.GetType().PrettyName();
        var responseName = typeof(TResponse).PrettyName();

        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Handling {RequestType} => {ResponseType}", requestName, responseName);
       
        var response = await next();

        var duration = stopwatch.Elapsed;

        logger.LogInformation("Handled {RequestType} => {ResponseType} in {Duration}ms", requestName, responseName, duration.TotalMilliseconds);

        if (duration > options.Value.MaxExpectedDuration)
        {
            logger.LogWarning(
                "Handling {RequestType} => {ResponseType} took longer than expected: {Duration}ms",
                requestName, responseName, duration);

            var notification = new RequestExceedsExpectedDurationNotification(
                requestName,
                responseName,
                duration,
                typeof(PerformanceLoggingBehaviour<TRequest, TResponse>).PrettyName()
            );

            await notificationStore.StoreAsync(notification, cancellationToken);
        }

        return response;
    }
}
