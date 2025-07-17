using DA.Messaging.Requests.Behaviours;
using Messaging.Tests.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Tests.Integration;

public class PerformanceLoggingPipelineBehaviourTests
{
    [Fact]
    public async Task Pipeline_WithPerformanceLogging_EmitsNotification_WhenRequestIsSlow()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder().Build();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddRequestMessaging(options =>
        {
            options.FromAssemblyContaining<SlowSampleQueryHandler>();
            options.AddPerformanceLoggingBehavior(configure =>
            {
                configure.MaxExpectedDuration = TimeSpan.FromMilliseconds(50);
            });
        });

        services.AddNotificationMessaging(options => options.FromAssemblyContaining<TestNotificationHandler>());

        var sink = new InMemoryTestNoficationSink();
        services.AddSingleton<ITestNotificationSink>(sink);

        var provider = services.BuildServiceProvider();

        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();
        var publisher = provider.GetRequiredService<INotificationPublisher>();
        var store = provider.GetRequiredService<INotificationStore>();

        var result = await dispatcher.DispatchAsync(new SlowSampleQuery(TimeSpan.FromMilliseconds(75)), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.TryGetValue(out var value).ShouldBeTrue();
        value.ShouldBe("response from slow handler");

        // op dit moment zou de notification moeten zijn opgeslagen in de store
        var pendingNotifications = await store.GetPendingNotificationsAsync(CancellationToken.None);
        pendingNotifications.Count.ShouldBe(1);

        // nu publiceren we de notification
        await publisher.PublishAsync(CancellationToken.None);

        // en nu zou de notification handler deze moeten hebben verwerkt
        sink.Notifications.Count.ShouldBe(1);
        var notification = sink.Notifications.Single() as RequestExceedsExpectedDurationNotification;
        notification.ShouldNotBeNull();
        notification.RequestType.ShouldBe(nameof(SlowSampleQuery));
        notification.ResponseType.ShouldBe("Result<String>");
        notification.Duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(75));
    }
}