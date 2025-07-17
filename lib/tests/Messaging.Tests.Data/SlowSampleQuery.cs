namespace Messaging.Tests.Data;

public class SlowSampleQuery : IQuery<string>
{
    public TimeSpan Delay { get; }
    public SlowSampleQuery(TimeSpan delay)
    {
        Delay = delay;
    }
}