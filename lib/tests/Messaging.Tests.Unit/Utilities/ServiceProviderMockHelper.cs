namespace Messaging.Tests.Unit.Utilities;

internal static class ServiceProviderMockHelper
{
    public static IServiceProvider WithService<TService>(this IServiceProvider provider, TService implementation)
        where TService : class
    {
        provider.GetService(typeof(TService)).Returns(implementation);
        return provider;
    }

    public static IServiceProvider WithEnumerableService<TService>(this IServiceProvider provider, params TService[] implementations)
        where TService : class
    {
        var serviceType = typeof(TService);
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
        provider.GetService(enumerableType).Returns(implementations);
        return provider;
    }
}