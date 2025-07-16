using System.Reflection;
using DA.Messaging.Notifications;
using DA.Messaging.Notifications.Abstractions;

namespace DA.Messaging.DependencyInjection;

public sealed class NotificationMessagingOptions
{
    internal List<Assembly> HandlerAssemblies { get; } = [];
    internal Type? CustomNotificationStoreType { get; private set; }

    /// <summary>
    /// Add a single assembly to scan for notification handlers.
    /// </summary>
    /// <returns>The NotificationMessagingOptions for fluent configuration.</returns>
    public NotificationMessagingOptions FromAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));
        HandlerAssemblies.Add(assembly);
        return this;
    }

    /// <summary>
    /// Add multiple assemblies to scan for notification handlers.
    /// </summary>
    /// <returns>The NotificationMessagingOptions for fluent configuration.</returns>
    public NotificationMessagingOptions FromAssemblies(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies, nameof(assemblies));
        HandlerAssemblies.AddRange(assemblies);
        return this;
    }

    /// <summary>
    /// Add the assembly that contains the specified type to scan for notification handlers.
    /// </summary>
    /// <returns>The NotificationMessagingOptions for fluent configuration.</returns>
    public NotificationMessagingOptions FromAssemblyContaining<T>()
    {
        ArgumentNullException.ThrowIfNull(typeof(T), nameof(T));
        return FromAssembly(typeof(T).Assembly);
    }

    /// <summary>
    /// Use a custom <see cref="INotificationStore"/> as the store for the notifications.
    /// If not set, the <see cref="InMemoryNotificationStore"/> will be used.
    /// </summary>
    /// <returns>The NotificationMessagingOptions for fluent configuration.</returns>
    public NotificationMessagingOptions UseCustomNotificationStore<TNotificationStore>()
        where TNotificationStore : class, INotificationStore
    {
        CustomNotificationStoreType = typeof(TNotificationStore);
        return this;
    }
}
