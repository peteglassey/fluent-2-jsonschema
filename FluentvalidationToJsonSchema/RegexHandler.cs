using System.Reflection;
using FluentValidation.Validators;

namespace FluentvalidationToJsonSchema
{
    public class RegexHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator is IRegularExpressionValidator;

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            var regexValidator = (IRegularExpressionValidator)validator;
            propertySchema.Pattern = regexValidator.Expression;
        }
    }
} 