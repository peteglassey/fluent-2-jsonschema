using System.Reflection;

namespace FluentvalidationToJsonSchema
{
    public class EnumHandler : IFluentValidatorSchemaHandler
    {
        public bool CanHandle(object validator) => validator.GetType().Name.Contains(typeof(FluentValidation.Validators.EnumValidator<,>).Name);

        public void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema)
        {
            if (propertyInfo.PropertyType.IsEnum && propertySchema.Enum == null)
            {
                var enumValues = System.Enum.GetValues(propertyInfo.PropertyType).Cast<object>().ToArray();
                propertySchema.Enum = enumValues;
            }
        }
    }
} 