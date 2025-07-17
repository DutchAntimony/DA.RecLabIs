using DA.Messaging.Notifications;
using DA.Messaging.Notifications.Abstractions;
using DA.Messaging.Requests;
using DA.Messaging.Requests.Abstractions;
using DA.Messaging.Requests.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;

namespace DA.Messaging.DependencyInjection;
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Add notification messaging services to the service collection with 
    /// the default configuration and without assemblies to scan.
    /// </summary>
    public static IServiceCollection AddNotificationMessaging(this IServiceCollection services) => services.AddNotificationMessaging(_ => { /* use defaults */ });

    /// <summary>
    /// Add notification messaging services to the service collection with custom configuration.
    /// </summary>
    /// <param name="configure">Delegate to configure the options for the notification messaging.</param>
    public static IServiceCollection AddNotificationMessaging(this IServiceCollection services, Action<NotificationMessagingOptions> configure)
    {
        var options = new NotificationMessagingOptions();
        configure(options);

        services.TryAddSingleton<INotificationPublisher, NotificationPublisher>();
        // Register a fallback no-op logger if none is provided because NotificationPublisher requires an ILogger
        services.TryAddSingleton(typeof(ILogger<>), typeof(NullLogger<>));

        // Register all handlers from the specified assemblies
        foreach (var assembly in options.HandlerAssemblies)
        {
            services.AddNotificationHandlersFromAssembly(assembly);
        }

        if (options.CustomNotificationStoreType is not null)
        {
            // Register the custom notification store type if provided. This is registered as scoped to ensure compatibility with e.g. EF Core.
            services.TryAddScoped(typeof(INotificationStore), options.CustomNotificationStoreType);
        }
        else
        {
            /// Register the in-memory notification store as the default implementation. This is registered as singleton because the in memory collection contains state that is stored over the lifeteime of requests.
            services.TryAddSingleton<INotificationStore, InMemoryNotificationStore>();
        }

        return services;
    }

    /// <summary>
    /// Add request messaging services to the service collection with 
    /// the default configuration and without assemblies to scan.
    /// </summary>
    public static IServiceCollection AddRequestMessaging(this IServiceCollection services) => services.AddRequestMessaging(_ => { /* use defaults */ });

    /// <summary>
    /// Add request messaging services to the service collection with custom configuration
    /// </summary>
    /// <param name="configure">Delegate to configure the options for the request messaging.</param>
    public static IServiceCollection AddRequestMessaging(this IServiceCollection services, Action<RequestMessagingOptions> configure)
    {
        var options = new RequestMessagingOptions();
        configure(options);

        services.TryAddSingleton<IRequestDispatcher, RequestDispatcher>();

        // Register a fallback no-op logger if none is provided because RequestDispatcher requires an ILogger
        services.TryAddSingleton(typeof(ILogger<>), typeof(NullLogger<>));

        foreach (var assembly in options.HandlerAssemblies)
        {
            services.AddRequestHandlersFromAssembly(assembly);
        }

        foreach (var behaviorType in options.PipelineBehaviours)
        {
            services.AddScoped(typeof(IRequestPipelineBehaviour<,>), behaviorType);
        }

        options.ConfigureAdditionalServices?.Invoke(services);

        return services;
    }
   
    private static void AddNotificationHandlersFromAssembly(this IServiceCollection services, Assembly handlerAssembly)
    {
        var handlerTypes = handlerAssembly
                        .GetTypes()
                        .Where(t => !t.IsAbstract && !t.IsInterface)
                        .SelectMany(t => t.GetInterfaces(), (t, i) => new { Interface = i, Implementation = t })
                        .Where(x => x.Interface.IsGenericType && x.Interface.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                        .ToList();

        foreach (var registration in handlerTypes)
        {
            services.TryAddScoped(registration.Interface, registration.Implementation);
        }
    }

    private static void AddRequestHandlersFromAssembly(this IServiceCollection services, Assembly handlerAssembly)
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
