using DA.Messaging.DependencyInjection;
using DA.Messaging.Requests.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DA.Messaging.Validation;

/// <summary>
/// Extensions for <see cref="RequestMessagingOptions"/> to add validation behaviour.
/// </summary>
public static class RequestMessagingOptionsValidationExtensions
{
    /// <summary>
    /// Add a pipeline behaviour that validates requests using FluentValidation.
    /// This will register all validators found in the specified handler assemblies.
    /// </summary>
    /// <param name="options">Extensions for RequestMessagingOptions</param>
    /// <returns>The RequestMessagingOptions for fluent configuration.</returns>
    public static RequestMessagingOptions AddValidationBehaviour(this RequestMessagingOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.ConfigureAdditionalServices += services =>
        {
            services.AddValidatorsFromAssemblies(options.HandlerAssemblies, includeInternalTypes:true); // registreert IValidator<T>
            services.AddScoped(typeof(IRequestPipelineBehaviour<,>), typeof(ValidationPipelineBehaviour<,>));
        };

        return options;
    }
}
