using DA.Optional;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

internal static class OptionShouldHelpers
{
    public static bool ShouldHaveValue<T>(this Option<T> option, T expected)
    {
        option.TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldBe(expected);
        return true;
    }

    public static bool ShouldContain(this Option<string> option, string expected)
    {
        option.TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldContain(expected);
        return true;
    }
}