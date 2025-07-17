using Messaging.Tests.Unit.Requests.Samples;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Tests.Unit.Requests;

public partial class RequestDispatcherTests
{

    [Fact]
    public async Task Should_Dispatch_Handler_On_First_Use()
    {
        var dispatcher = BuildDispatcherWithHandler();
        var query = new SampleQuery();

        var result = await dispatcher.DispatchAsync(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.TryGetValue(out var value).ShouldBeTrue();
        value.ShouldBe("response from handler");
    }

    [Fact]
    public async Task Should_Dispatch_Handler_On_Second_Use_Too()
    {
        var dispatcher = BuildDispatcherWithHandler();
        var query1 = new SampleQuery();
        var query2 = new SampleQuery();

        var result1 = await dispatcher.DispatchAsync(query1);
        var result2 = await dispatcher.DispatchAsync(query2);

        result1.IsSuccess.ShouldBeTrue();
        result2.TryGetValue(out var value).ShouldBeTrue();
        value.ShouldBe("response from handler");
    }

    [Fact]
    public async Task Should_Throw_When_Handler_Not_Registered()
    {
        var dispatcher = BuildDispatcherWithoutHandler();
        var query = new SampleQuery();

        var ex = await Should.ThrowAsync<InvalidOperationException>(() =>
            dispatcher.DispatchAsync(query));

        ex.Message.ShouldContain("No service for type");
    }

    private static IRequestDispatcher BuildDispatcherWithHandler()
    {
        var services = new ServiceCollection();

        services.AddRequestMessaging(options => options.FromAssemblyContaining<SampleQueryHandler>());

        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IRequestDispatcher>();
    }

    private static IRequestDispatcher BuildDispatcherWithoutHandler()
    {
        var services = new ServiceCollection();

        services.AddRequestMessaging();

        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IRequestDispatcher>();
    }
}