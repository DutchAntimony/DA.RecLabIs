namespace Optional.Tests.Unit;

public class OptionTestBase
{
    protected const string OptionValue = "abc"; // todo: rename to Value

    protected static readonly Option<string> SomeOption = OptionValue;
    protected static readonly Option<string> NoneOption = Option.None;

    protected readonly Task<Option<string>> SomeOptionTask= Task.FromResult(SomeOption);
    protected readonly Task<Option<string>> NoneOptionTask = Task.FromResult(NoneOption);
}
