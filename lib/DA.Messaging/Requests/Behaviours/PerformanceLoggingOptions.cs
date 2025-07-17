namespace DA.Messaging.Requests.Behaviours;

/// <summary>
/// Options for configuring the performance logging behaviour.
/// </summary>
public class PerformanceLoggingOptions
{
    /// <summary>
    /// The maximum expected duration for handling a request.
    /// If the duration exceeds this value, a notification will be raised.
    /// </summary>
    public TimeSpan MaxExpectedDuration { get; set; } = TimeSpan.FromSeconds(5);

    internal static string ConfigSectionName => "PerformanceLogging";
}
