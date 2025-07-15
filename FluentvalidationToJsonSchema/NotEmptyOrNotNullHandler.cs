using System.Reflection;
using FluentValidation.Validators;

namespace FluentvalidationToJsonSchema
{
    public class NotEmptyOrNotNullHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator is INotEmptyValidator || validator is INotNullValidator;

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            // The required list is on the parent JsonSchemaObject, so this handler will need to be called differently.
            // For now, we can set a flag on the propertySchema (e.g., IsRequired) and let the main loop collect required fields after all handlers run.
            propertySchema.GetType().GetProperty("IsRequired")?.SetValue(propertySchema, true);
        }
    }
} 