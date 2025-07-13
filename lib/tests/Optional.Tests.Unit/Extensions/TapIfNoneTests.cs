namespace Optional.Tests.Unit.Extensions;

public class TapIfNoneTests : OptionTestBase
{
    private int _testValue = 0;

    private void SomeAction() => _testValue++;

    private Task SomeActionTask()
    {
        SomeAction();
        return Task.CompletedTask;
    }

    private static void ThrowingAction() => throw new ShouldAssertException("Should not be called");

    private static Task ThrowingActionTask()
    {
        ThrowingAction();
        return Task.CompletedTask;
    }

    [Fact]
    public void TapIfNone_Should_ApplyAction_WhenOptionIsNone()
    {
        _testValue = 0;
        NoneOption.TapIfNone(SomeAction);
        _testValue.ShouldBe(1);
    }

    [Fact]
    public void TapIfNone_Should_NotApplyAction_WhenOptionIsSome()
    {
        Should.NotThrow(() => SomeOption.TapIfNone(ThrowingAction));
    }

    [Fact]
    public async Task TapIfNone_Should_BehaveCorrectlyWithAsyncOverloads()
    {
        await Should.NotThrowAsync(() => SomeOption.TapIfNoneAsync(ThrowingActionTask));

        await Should.NotThrowAsync(() => SomeOptionTask.TapIfNone(ThrowingAction));

        await Should.NotThrowAsync(() => SomeOptionTask.TapIfNoneAsync(ThrowingActionTask));

        _testValue = 0;
        await NoneOptionTask.TapIfNone(SomeAction);
        _testValue.ShouldBe(1);


        _testValue = 0;
        await NoneOptionTask.TapIfNoneAsync(SomeActionTask);
        _testValue.ShouldBe(1);
    }
}