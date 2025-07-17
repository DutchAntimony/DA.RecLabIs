using DA.Optional;
using DA.Results;
using DA.Results.Extensions;

namespace DA.Messaging.Notifications;

/// <summary>
/// Result of processing a notification
/// </summary>
public record NotificationProcessingResult
{
    /// <summary>
    /// Notification is not processed yet.
    /// </summary>
    public static NotificationProcessingResult NotProcessed { get; } = new(false, DateTime.MinValue, string.Empty, Option.None);

    /// <summary>
    /// Indicates whether the notification was processed.
    /// </summary>
    public bool IsProcessed { get; }

    /// <summary>
    /// Indicates whether the notification was processed successfully.
    /// </summary>
    public bool IsSuccessful => IsProcessed && !ErrorMessage.HasValue;

    /// <summary>
    /// The date and time when the notification was processed.
    /// </summary>
    public DateTime ProcessedAt { get; }

    /// <summary>
    /// The processor that handled the notification.
    /// </summary>
    public string ProcessedBy { get; }

    /// <summary>
    /// Error message if the processing failed.
    /// </summary>
    public Option<string> ErrorMessage { get; }

    /// <summary>
    /// Indicate that the notification was successfully processed.
    /// </summary>
    /// <param name="processedBy">The service that processed the notification.</param>
    /// <param name="processedAt">Optionally the date time the the notification was processed. Default to UtcNow.</param>
    /// <returns>A successful <see cref="NotificationProcessingResult"/></returns>
    public static NotificationProcessingResult Success(string processedBy, DateTime? processedAt = null) =>
        new(true, processedAt ?? DateTime.UtcNow, processedBy, Option.None);

    /// <summary>
    /// Indicate that the notification could not be processed.
    /// </summary>
    /// <param name="processedBy">The service that processed the notification.</param>
    /// <param name="processedAt">Optionally the date time the the notification was processed. Default to UtcNow.</param>
    /// <param name="errorMessage">The message indicating why the processing failed.</param>
    /// <returns>A failing <see cref="NotificationProcessingResult"/></returns>
    public static NotificationProcessingResult Failure(string processedBy, string errorMessage, DateTime? processedAt = null) =>
        new(true, processedAt ?? DateTime.UtcNow, processedBy, errorMessage);

    /// <summary>
    /// Create a <see cref="NotificationProcessingResult"/> from a <see cref="Result"/>.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> that indicates if the notification succeeded or failed.</param>
    /// <param name="processedBy">The service that processed the notification.</param>
    /// <param name="processedAt">Optionally the date time the the notification was processed. Default to UtcNow.</param>
    /// <returns>A new <see cref="NotificationProcessingResult"/></returns>
    public static NotificationProcessingResult FromResult(Result result, string processedBy, DateTime? processedAt = null) =>
        result.Match(
            () => Success(processedBy, processedAt),
            error => Failure(processedBy, error.Message, processedAt));

    private NotificationProcessingResult(bool isProcessed, DateTime processedAt, string processedBy, Option<string> errorMessage) =>
        (IsProcessed, ProcessedAt, ProcessedBy, ErrorMessage) = (isProcessed, processedAt, processedBy, errorMessage);
}
