using System.Reflection;

namespace FluentvalidationToJsonSchema
{
    public class InclusiveBetweenHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator.GetType().Name.Contains(typeof(FluentValidation.Validators.InclusiveBetweenValidator<,>).Name);

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            var fromValue = validator.GetFrom();
            var toValue = validator.GetTo();
            if (fromValue != null && toValue != null)
            {
                if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(long) || propertyInfo.PropertyType == typeof(short))
                {
                    if (fromValue is IConvertible && toValue is IConvertible)
                    {
                        var min = System.Convert.ToInt64(fromValue);
                        var max = System.Convert.ToInt64(toValue);
                        propertySchema.ApplyMinimum((int)min);
                        propertySchema.ApplyMaximum((int)max);
                    }
                }
            }
        }
    }
} 