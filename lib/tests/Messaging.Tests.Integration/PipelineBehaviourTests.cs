using Messaging.Tests.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Tests.Integration;
public class PipelineBehaviourTests
{
    [Fact]
    public async Task RequestMessaging_Should_Invoke_CustomPipelineBehavior_Registered_Via_Options()
    {
        var log = new List<string>();

        var services = new ServiceCollection();

        services.AddSingleton(log); // Inject the log for inspection
        services.AddRequestMessaging(options =>
        {
            options.FromAssemblyContaining<SampleQueryHandler>();
            options.AddPipelineBehaviour(typeof(TrackingBehavior<,>)); // Register via options
        });

        var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();

        var result = await dispatcher.DispatchAsync(new SampleQuery());

        result.IsSuccess.ShouldBeTrue();
        result.TryGetValue(out var response).ShouldBeTrue();
        response.ShouldBe("response from handler"); // Assuming your handler returns this

        log.ShouldBe(["Before-", "After-"],
            ignoreOrder: false,
            customMessage: "TrackingBehavior should have added entries before and after.");
    }
}
