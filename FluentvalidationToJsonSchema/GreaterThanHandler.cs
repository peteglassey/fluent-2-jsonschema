using System.Reflection;

namespace FluentvalidationToJsonSchema
{
    public class GreaterThanHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator.GetType().Name.Contains(typeof(FluentValidation.Validators.GreaterThanValidator<,>).Name);

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            var value = validator.GetValueToCompare();
            if (value is int i)
            {
                var min = i + 1;
                propertySchema.ApplyMinimum(min);
            }
        }
    }
} 