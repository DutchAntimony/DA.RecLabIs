using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace DA.Messaging.Infrastructure;

/// <summary>
/// Null logger implementation for ILogger<T> that does nothing.
/// </summary>
/// <typeparam name="TCategoryName">The type whose name is used for the logger category name.</typeparam>
[ExcludeFromCodeCoverage]
internal sealed class NullLogger<TCategoryName> : ILogger<TCategoryName>
{
    public bool IsEnabled(LogLevel logLevel) => false;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        // Intentionally no-op
    }

    /// <summary>
    /// Null scope implementation that does nothing.
    /// </summary>
    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose() { }
    }

    IDisposable ILogger.BeginScope<TState>(TState state) => NullScope.Instance;
}