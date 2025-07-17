using FluentValidation;

namespace Messaging.Tests.Data;

internal sealed class SampleQueryValidator : AbstractValidator<SampleQuery>
{
    public SampleQueryValidator()
    {
        RuleFor(x => x.Input).MaximumLength(10);
    }
}