using System.Reflection;
using FluentValidation.Validators;

namespace FluentvalidationToJsonSchema
{
    public class LengthHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator is ILengthValidator;

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            var v = (ILengthValidator)validator;
            if (v.Min > 0)
                propertySchema.ApplyMinLength(v.Min);
            if (v.Max > 0)
                propertySchema.ApplyMaxLength(v.Max);
        }
    }
} 