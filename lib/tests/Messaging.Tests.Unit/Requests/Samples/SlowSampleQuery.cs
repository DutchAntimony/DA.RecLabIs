namespace Messaging.Tests.Unit.Requests.Samples;

internal class SlowSampleQuery : IQuery<string>
{
    public TimeSpan Delay { get; }
    public SlowSampleQuery(TimeSpan delay)
    {
        Delay = delay;
    }
}