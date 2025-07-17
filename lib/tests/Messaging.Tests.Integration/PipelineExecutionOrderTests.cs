using DA.Messaging.Requests.Behaviours;
using Messaging.Tests.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Tests.Integration;

public class PipelineExecutionOrderTests
{
    [Fact]
    public async Task Pipeline_Should_Invoke_Behaviors_In_Reverse_Order()
    {
        var log = new List<string>();

        var services = new ServiceCollection();
        services.AddRequestMessaging(options =>
        {
            options.FromAssemblyContaining<SampleQueryHandler>();
            options.ConfigureAdditionalServices += s =>
            {
                s.AddSingleton<IRequestPipelineBehaviour<SampleQuery, Result<string>>>(new TrackingBehavior<SampleQuery, Result<string>>(log, "A"));
                s.AddSingleton<IRequestPipelineBehaviour<SampleQuery, Result<string>>>(new TrackingBehavior<SampleQuery, Result<string>>(log, "B"));
            };
        });

        var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();

        await dispatcher.DispatchAsync(new SampleQuery());

        log.ShouldBe([
            "Before-A",
            "Before-B",
            "After-B",
            "After-A"
        ]);
    }
}
