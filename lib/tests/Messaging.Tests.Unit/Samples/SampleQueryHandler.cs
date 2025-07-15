namespace Messaging.Tests.Unit.Samples;

internal class SampleQueryHandler : IQueryHandler<SampleQuery, string>
{
    public Task<Result<string>> HandleAsync(SampleQuery request, CancellationToken cancellationToken)
        => Task.FromResult(Result.Success("response from handler"));
}