using DA.Messaging.Notifications;
using DA.Messaging.Notifications.Abstractions;
using Messaging.Tests.Unit.Notifications.Samples;
using Messaging.Tests.Unit.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Messaging.Tests.Unit.Notifications;

public class NotificationPublisherTests
{
    private readonly SampleNotification _notification = new("payload");
    [Fact]
    public async Task PublishAsync_Should_InvokeHandler_AndMarkAsPublished()
    {
        // Arrange
        var store = Substitute.For<INotificationStore>();
        store.GetPendingNotificationsAsync(Arg.Any<CancellationToken>())
            .Returns([_notification]);

        var handler = Substitute.For<INotificationHandler<SampleNotification>>();
        handler.HandleAsync(_notification, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider
            .WithService(store)
            .WithEnumerableService<INotificationHandler<SampleNotification>>(handler);

        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(serviceProvider);

        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory.CreateScope().Returns(scope);

        var publisher = new NotificationPublisher(scopeFactory, NullLogger<NotificationPublisher>.Instance);

        await publisher.PublishAsync();

        await handler.Received(1).HandleAsync(_notification, Arg.Any<CancellationToken>());
        await store.Received(1).MarkAsPublishedAsync(_notification.Id, Arg.Any<NotificationProcessingResult>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PublishAsync_Should_MarkAsFailed_WhenHandlerThrows()
    {
        var store = Substitute.For<INotificationStore>();
        store.GetPendingNotificationsAsync(Arg.Any<CancellationToken>())
            .Returns([_notification]);

        var handler = Substitute.For<INotificationHandler<SampleNotification>>();
        handler
            .HandleAsync(_notification, Arg.Any<CancellationToken>())
            .Throws(new InvalidOperationException("Something went wrong"));

        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider
            .WithService(store)
            .WithEnumerableService<INotificationHandler<SampleNotification>>(handler);

        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(serviceProvider);

        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory.CreateScope().Returns(scope);

        var publisher = new NotificationPublisher(scopeFactory, NullLogger<NotificationPublisher>.Instance);

        await publisher.PublishAsync();

        await handler.Received(1).HandleAsync(_notification, Arg.Any<CancellationToken>());
        await store.Received(1).MarkAsPublishedAsync(
            _notification.Id,
            Arg.Is<NotificationProcessingResult>(r => !r.IsSuccessful && r.ErrorMessage.ShouldContain("An unexpected error occurred")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PublishAsync_Should_Handle_No_Handlers_Registered()
    {
        var store = Substitute.For<INotificationStore>();
        store.GetPendingNotificationsAsync(Arg.Any<CancellationToken>())
            .Returns([_notification]);

        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider
            .WithService(store)
            .WithEnumerableService<INotificationHandler<SampleNotification>>([]);

        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(serviceProvider);

        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory.CreateScope().Returns(scope);

        var publisher = new NotificationPublisher(scopeFactory, NullLogger<NotificationPublisher>.Instance);

        // Act
        await publisher.PublishAsync();

        // Assert
        await store.Received(1).MarkAsPublishedAsync(
            _notification.Id,
            Arg.Is<NotificationProcessingResult>(r => !r.IsSuccessful && r.ErrorMessage.ShouldContain("No handlers")),
            Arg.Any<CancellationToken>());
    }
}