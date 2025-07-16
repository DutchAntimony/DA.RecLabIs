using System.Reflection;

namespace DA.Messaging.DependencyInjection;

public sealed class RequestMessagingOptions
{
    internal List<Assembly> HandlerAssemblies { get; } = [];

    /// <summary>
    /// Add a single assembly to scan for request handlers.
    /// </summary>
    /// <returns>The RequestMessagingOptions for fluent configuration.</returns>
    public RequestMessagingOptions FromAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));
        HandlerAssemblies.Add(assembly);
        return this;
    }

    /// <summary>
    /// Add multiple assemblies to scan for request handlers.
    /// </summary>
    /// <returns>The RequestMessagingOptions for fluent configuration.</returns>
    public RequestMessagingOptions FromAssemblies(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies, nameof(assemblies));
        HandlerAssemblies.AddRange(assemblies);
        return this;
    }

    /// <summary>
    /// Add the assembly that contains the specified type to scan for request handlers.
    /// </summary>
    /// <returns>The RequestMessagingOptions for fluent configuration.</returns>
    public RequestMessagingOptions FromAssemblyContaining<T>()
    {
        ArgumentNullException.ThrowIfNull(typeof(T), nameof(T));
        return FromAssembly(typeof(T).Assembly);
    }
}
