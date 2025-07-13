namespace Optional.Tests.Unit.Extensions;

public class TapTests : OptionTestBase
{
    private int _testValue = 0;

    private void SomeAction(string original) => _testValue++;

    private Task SomeActionTask(string original)
    {
        SomeAction(original);
        return Task.CompletedTask;
    }

    private static void ThrowingAction(string original) => throw new ShouldAssertException("Should not be called");

    private static Task ThrowingActionTask(string original)
    {
        ThrowingAction(original);
        return Task.CompletedTask;
    }

    [Fact]
    public void Tap_Should_ApplyAction_WhenOptionIsSome()
    {
        _testValue = 0;
        SomeOption.Tap(SomeAction);
        _testValue.ShouldBe(1);
    }

    [Fact]
    public void Tap_Should_NotApplyAction_WhenOptionIsNone()
    {
        Should.NotThrow(() => NoneOption.Tap(ThrowingAction));
    }

    [Fact]
    public async Task Tap_Should_BehaveCorrectlyWithAsyncOverloads()
    {
        await Should.NotThrowAsync(() => NoneOption.TapAsync(ThrowingActionTask));

        await Should.NotThrowAsync(() => NoneOptionTask.Tap(ThrowingAction));

        await Should.NotThrowAsync(() => NoneOptionTask.TapAsync(ThrowingActionTask));

        _testValue = 0;
        await SomeOptionTask.Tap(SomeAction);
        _testValue.ShouldBe(1);

        _testValue = 0;
        await SomeOptionTask.TapAsync(SomeActionTask);
        _testValue.ShouldBe(1);
    }
}