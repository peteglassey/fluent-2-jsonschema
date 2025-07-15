using System.Reflection;
using FluentValidation.Validators;

namespace FluentvalidationToJsonSchema
{
    public class LessThanOrEqualHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator is ILessThanOrEqualValidator;

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            var value = validator.GetValueToCompare();
            if (value is int i)
                propertySchema.ApplyMaximum(i);
        }
    }
} 