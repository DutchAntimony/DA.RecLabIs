namespace Messaging.Tests.Unit.Utilities;

internal static class OptionShouldHelpers
{
    public static bool ShouldContain(this Option<string> option, string expected)
    {
        option.TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldContain(expected);
        return true; // if false, the assertions will fail
    }
}