using DA.Messaging.Requests.Behaviours;

namespace Messaging.Tests.Data;

public sealed class TrackingBehavior<TRequest, TResponse>(List<string> log, string name = "")
    : IRequestPipelineBehaviour<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        log.Add($"Before-{name}");
        var result = await next();
        log.Add($"After-{name}");
        return result;
    }
}