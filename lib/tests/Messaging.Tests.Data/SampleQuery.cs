namespace Messaging.Tests.Data;

public class SampleQuery : IQuery<string>
{
    public string Input { get; set; } = "default";
}
