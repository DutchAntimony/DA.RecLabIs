using Messaging.Tests.Unit.Samples;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Tests.Unit.DependencyInjection;

public class DependencyInjectionExtensionsTests
{
    [Fact]
    public void AddMessaging_ShouldAddRequestDispatcher()
    {
        var services = new ServiceCollection();

        services.AddMessaging();

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<IRequestDispatcher>().ShouldNotBeNull();
    }

    [Fact]
    public void AddMessagingWithHandlersFromAssembly_ShouldAddHandlers()
    {
        var services = new ServiceCollection();

        services.AddMessagingWithHandlersFromAssembly(typeof(SampleQuery).Assembly);

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<IRequestDispatcher>().ShouldNotBeNull();
        serviceProvider.GetService<IRequestHandler<SampleQuery, Result<string>>>().ShouldNotBeNull();
    }

    [Fact]
    public void AddMessagingWithHandlersFromAssemblies_ShouldAddHandlers()
    {
        var services = new ServiceCollection();

        services.AddMessagingWithHandlersFromAssemblies(typeof(SampleQuery).Assembly);

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<IRequestDispatcher>().ShouldNotBeNull();
        serviceProvider.GetService<IRequestHandler<SampleQuery, Result<string>>>().ShouldNotBeNull();
    }

    [Fact]
    public void AddMessagingWithHandlersFromAssemblyContaining_ShouldAddHandlers()
    {
        var services = new ServiceCollection();

        services.AddMessagingWithHandlersFromAssemblyContaining<SampleQuery>();

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<IRequestDispatcher>().ShouldNotBeNull();
        serviceProvider.GetService<IRequestHandler<SampleQuery, Result<string>>>().ShouldNotBeNull();
    }
}
