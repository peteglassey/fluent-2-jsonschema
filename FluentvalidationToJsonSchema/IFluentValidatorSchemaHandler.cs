using System.Reflection;

namespace FluentvalidationToJsonSchema
{
    public interface IFluentValidatorSchemaHandler
    {
        bool CanHandle(object validator);
        void Apply(object validator, PropertyInfo propertyInfo, JsonSchemaProperty propertySchema);
    }
} 