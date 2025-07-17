namespace Messaging.Tests.Data;

public class SlowSampleQueryHandler : IQueryHandler<SlowSampleQuery, string>
{
    public async Task<Result<string>> HandleAsync(SlowSampleQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(request.Delay, cancellationToken);
        return Result.Success("response from slow handler");
    }
}