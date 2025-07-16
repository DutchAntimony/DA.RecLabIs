namespace Messaging.Tests.Unit.Notifications;

public class NotificationProcessingResultTests
{
    private readonly string _defaultProcessor = "DefaultProcessor";
    private readonly string _errorMessage = "No handlers registered for this notification type.";

    [Fact]
    public void NotProcessed_Should_HaveDefaultValues()
    {
        var result = NotificationProcessingResult.NotProcessed;

        result.IsProcessed.ShouldBeFalse();
        result.IsSuccessful.ShouldBeFalse();
        result.ProcessedAt.ShouldBe(DateTime.MinValue);
        result.ProcessedBy.ShouldBe(string.Empty);
        result.ErrorMessage.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void Success_Should_SetValuesCorrectly()
    {
        var result = NotificationProcessingResult.Success(_defaultProcessor);

        result.IsProcessed.ShouldBeTrue();
        result.IsSuccessful.ShouldBeTrue();
        result.ProcessedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        result.ProcessedBy.ShouldBe(_defaultProcessor);
        result.ErrorMessage.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void Success_Should_SetValuesCorrectly_WithCustomDateTime()
    {
        var dateTime = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        var result = NotificationProcessingResult.Success(_defaultProcessor, dateTime);

        result.IsProcessed.ShouldBeTrue();
        result.IsSuccessful.ShouldBeTrue();
        result.ProcessedAt.ShouldBe(dateTime);
        result.ProcessedBy.ShouldBe(_defaultProcessor);
        result.ErrorMessage.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void Failure_Should_SetValuesCorrectly()
    {
        var result = NotificationProcessingResult.Failure(_defaultProcessor, _errorMessage);
        
        result.IsProcessed.ShouldBeTrue();
        result.IsSuccessful.ShouldBeFalse();
        result.ProcessedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        result.ProcessedBy.ShouldBe(_defaultProcessor);
        result.ErrorMessage.TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldBe(_errorMessage);
    }

    [Fact]
    public void Failure_Should_SetValuesCorrectly_WithCustomDateTime()
    {
        var dateTime = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        var result = NotificationProcessingResult.Failure(_defaultProcessor, _errorMessage, dateTime);

        result.IsProcessed.ShouldBeTrue();
        result.IsSuccessful.ShouldBeFalse();
        result.ProcessedAt.ShouldBe(dateTime);
        result.ProcessedBy.ShouldBe(_defaultProcessor);
        result.ErrorMessage.TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldBe(_errorMessage);
    }

    [Fact]
    public void FromResult_Should_ReturnSuccess_WhenResultIsSuccessful()
    {
        var result = Result.Success();

        var processingResult = NotificationProcessingResult.FromResult(result, _defaultProcessor);

        processingResult.IsProcessed.ShouldBeTrue();
        processingResult.IsSuccessful.ShouldBeTrue();
        processingResult.ProcessedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        processingResult.ProcessedBy.ShouldBe(_defaultProcessor);
        processingResult.ErrorMessage.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void FromResult_Should_ReturnFailure_WhenResultIsFailed()
    {
        var error = new DomainError(_errorMessage);
        var result = Result.Failure(error);

        var processingResult = NotificationProcessingResult.FromResult(result, _defaultProcessor);
        processingResult.IsProcessed.ShouldBeTrue();
        processingResult.IsSuccessful.ShouldBeFalse();
        processingResult.ProcessedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        processingResult.ProcessedBy.ShouldBe(_defaultProcessor);
        processingResult.ErrorMessage.TryGetValue(out var actual).ShouldBeTrue();
        actual.ShouldBe(_errorMessage);
    }
}
