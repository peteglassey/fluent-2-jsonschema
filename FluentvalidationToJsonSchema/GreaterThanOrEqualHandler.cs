using System.Reflection;
using FluentValidation.Validators;

namespace FluentvalidationToJsonSchema
{
    public class GreaterThanOrEqualHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator is IGreaterThanOrEqualValidator;

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            var value = validator.GetValueToCompare();
            if (value is int i)
                propertySchema.ApplyMinimum(i);
        }
    }
} 