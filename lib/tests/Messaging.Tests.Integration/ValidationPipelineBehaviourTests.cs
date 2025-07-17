using Messaging.Tests.Data;
using Microsoft.Extensions.DependencyInjection;
using DA.Messaging.Validation;

namespace Messaging.Tests.Integration;

public class ValidationPipelineBehaviourTests
{
    // This class is intended to test the validation pipeline behaviour.
    // It should contain tests that ensure that the validation pipeline behaves as expected.
    // For example, it should test that the validation pipeline correctly validates requests and notifications,
    // and that it throws the appropriate exceptions when validation fails.

    private readonly IServiceProvider _provider;
    
    public ValidationPipelineBehaviourTests()
    {
        var services = new ServiceCollection();

        services.AddRequestMessaging(options => options
            .FromAssemblyContaining<SampleQuery>()
            .AddValidationBehaviour());

        _provider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task ValidationPipelineBehaviour_Should_Succeeds_ForValidRequest()
    {
        var dispatcher = _provider.GetRequiredService<IRequestDispatcher>();

        var result = await dispatcher.DispatchAsync(new SampleQuery(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.TryGetValue(out var value).ShouldBeTrue();

        value.ShouldBe("response from handler");
    }

    [Fact]
    public async Task ValidationPipelineBehaviour_Should_Fail_ForInvalidRequest()
    {
        var dispatcher = _provider.GetRequiredService<IRequestDispatcher>();

        var result = await dispatcher.DispatchAsync(new SampleQuery() { Input = "Way too long"},
            CancellationToken.None);

        result.TryGetValue(out var _, out var error).ShouldBeFalse();

        var validationError = error as ValidationError;
        validationError.ShouldNotBeNull();

        validationError.Failures.ShouldNotBeEmpty();
        validationError.Failures.Single().PropertyName.ShouldBe(nameof(SampleQuery.Input));
    }
}
