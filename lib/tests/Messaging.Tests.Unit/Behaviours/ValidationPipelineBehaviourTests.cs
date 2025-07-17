using DA.Messaging.Requests.Behaviours;
using DA.Messaging.Validation;
using FluentValidation;
using FluentValidation.Results;
using Messaging.Tests.Data;
using Microsoft.Extensions.Logging;

namespace Messaging.Tests.Unit.Behaviours;

public class ValidationPipelineBehaviourTests
{
    [Fact]
    public async Task Should_Return_Success_WhenNoValidatorsAreRegistered()
    {
        var logger = Substitute.For<ILogger<ValidationPipelineBehaviour<TestRequest, Result>>>();

        var behaviour = new ValidationPipelineBehaviour<TestRequest, Result>(
            [], logger);

        var request = new TestRequest();
        var next = Substitute.For<RequestHandlerDelegate<Result>>();

        var result = await behaviour.HandleAsync(request, next, CancellationToken.None);

        result.ShouldBeNull(); // since there is no handler, we expect null result
    }

    [Fact]
    public async Task Should_Return_Result_With_ValidationError_When_Invalid_Request_With_NonGeneric_Result()
    {
        var validator = Substitute.For<IValidator<TestRequest>>();
        validator.ValidateAsync(Arg.Any<ValidationContext<TestRequest>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult([new FluentValidation.Results.ValidationFailure("Field", "Error")]));

        var logger = Substitute.For<ILogger<ValidationPipelineBehaviour<TestRequest, Result>>>();

        var behaviour = new ValidationPipelineBehaviour<TestRequest, Result>(
            [validator], logger);

        var request = new TestRequest();
        var next = Substitute.For<RequestHandlerDelegate<Result>>();

        var result = await behaviour.HandleAsync(request, next, CancellationToken.None);

        result.TryGetFailure(out var error).ShouldBeTrue();
        error.ShouldBeOfType<ValidationError>();
    }

    [Fact]
    public async Task Should_Return_ResultOfT_With_ValidationError_When_Invalid_Request_With_Generic_Result()
    {
        // Arrange
        var validator = Substitute.For<IValidator<SampleQuery>>();
        validator.ValidateAsync(Arg.Any<ValidationContext<SampleQuery>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult([new FluentValidation.Results.ValidationFailure("Field", "Error")]));

        var logger = Substitute.For<ILogger<ValidationPipelineBehaviour<SampleQuery, Result<string>>>>();

        var behaviour = new ValidationPipelineBehaviour<SampleQuery, Result<string>>(
            [validator], logger);

        var request = new SampleQuery();
        var next = Substitute.For<RequestHandlerDelegate<Result<string>>>();

        var result = await behaviour.HandleAsync(request, next, CancellationToken.None);

        result.TryGetFailure(out var error).ShouldBeTrue();
        error.ShouldBeOfType<ValidationError>();
    }

    [Fact]
    public async Task Should_Throw_When_Response_Type_Is_Not_Result()
    {
        var validator = Substitute.For<IValidator<TestStringRequest>>();
        validator.ValidateAsync(Arg.Any<ValidationContext<TestStringRequest>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Field", "Error") }));

        var logger = Substitute.For<ILogger<ValidationPipelineBehaviour<TestStringRequest, string>>>();

        var behaviour = new ValidationPipelineBehaviour<TestStringRequest, string>(
            [validator], logger);

        var request = new TestStringRequest();
        var next = Substitute.For<RequestHandlerDelegate<string>>();

        await Should.ThrowAsync<FluentValidation.ValidationException>(() =>
            behaviour.HandleAsync(request, next, CancellationToken.None));
    }

    public sealed class TestRequest : IRequest<Result> { }

    public sealed class TestStringRequest : IRequest<string> { }
}
