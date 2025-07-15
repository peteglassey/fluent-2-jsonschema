using System.Reflection;
using FluentValidation.Validators;

namespace FluentvalidationToJsonSchema
{
    public class EmailHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator is IEmailValidator;

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            propertySchema.Format = "email";
        }
    }
} 