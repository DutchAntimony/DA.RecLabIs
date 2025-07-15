using DA.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;

namespace DA.Messaging.DependencyInjection;
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Extension method to add DA.Messaging services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddRequestDispatcher();

        return services;
    }

    /// <summary>
    /// Extension method to add DA.Messaging services to the service collection.
    /// Also adds all request handlers from the specified assembly.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="handlerAssembly">The assembly of which the handlers should be registered.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMessagingWithHandlersFromAssembly(this IServiceCollection services, Assembly handlerAssembly)
    {
        services.AddRequestDispatcher();
        services.AddHandlersFromAssembly(handlerAssembly);
        return services;
    }

    /// <summary>
    /// Extension method to add DA.Messaging services to the service collection.
    /// Also adds all request handlers from the specified assembly.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="handlerAssemblies">The assemblies of which the handlers should be registered.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMessagingWithHandlersFromAssemblies(this IServiceCollection services, params Assembly[] handlerAssemblies)
    {
        services.AddRequestDispatcher();

        foreach (var assembly in handlerAssemblies)
        {
            services.AddHandlersFromAssembly(assembly);
        }

        return services;
    }

    /// <summary>
    /// Extension method to add DA.Messaging services to the service collection.
    /// Also adds all request handlers from the specified assemblies.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMessagingWithHandlersFromAssemblyContaining<THandlerAssembly>(this IServiceCollection services)
    {
        services.AddRequestDispatcher();
        services.AddHandlersFromAssembly(typeof(THandlerAssembly).Assembly);
        return services;
    }

    private static void AddRequestDispatcher(this IServiceCollection services)
    {
        services.TryAddSingleton<IRequestDispatcher, RequestDispatcher>();

        // Register a fallback no-op logger if none is provided because RequestDispatcher requires an ILogger
        services.TryAddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
    }

    private static void AddHandlersFromAssembly(this IServiceCollection services, Assembly handlerAssembly)
    {
        var handlerTypes = handlerAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t =>
                t.GetInterfaces()
                 .Where(i => i.IsGenericType && IsRequestHandlerInterface(i))
                 .Select(i => new { Interface = i, Implementation = t }))
            .Distinct();

        foreach (var registration in handlerTypes)
        {
            services.TryAddTransient(registration.Interface, registration.Implementation);
        }
    }

    private static bool IsRequestHandlerInterface(Type interfaceType)
    {
        var definition = interfaceType.GetGenericTypeDefinition();

        if (definition == typeof(IRequestHandler<,>))
            return true;

        // Traverse base interfaces recursively
        return interfaceType.GetInterfaces()
            .Any(baseInterface =>
                baseInterface.IsGenericType &&
                baseInterface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
    }
}
