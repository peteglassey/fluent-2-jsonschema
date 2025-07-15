using System.Reflection;

namespace FluentvalidationToJsonSchema
{
    public class LessThanHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator.GetType().Name.Contains(typeof(FluentValidation.Validators.LessThanValidator<,>).Name);

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            var value = validator.GetValueToCompare();
            if (value is int i)
            {
                var max = i - 1;
                propertySchema.ApplyMaximum(max);
            }
        }
    }
} 