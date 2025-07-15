using System;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.Validators;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentvalidationToJsonSchema
{
    public class FluentValidatorToJsonSchemaConverter
    {
        public JsonSchemaObject ConvertTo<T>(IValidator<T> validator)
        {
            var schema = new JsonSchemaObject();
            var descriptor = validator.CreateDescriptor();
            var type = typeof(T);

            // Restore manual handler registration
            var handlers = new List<IFluentValidatorSchemaHandler>
            {
                new GreaterThanOrEqualHandler(),
                new LessThanOrEqualHandler(),
                new LengthHandler(),
                new GreaterThanHandler(),
                new LessThanHandler(),
                new EnumHandler(),
                new RegexHandler(),
                new NotEmptyOrNotNullHandler(),
                new EmailHandler(),
                new InclusiveBetweenHandler(),
                new ExclusiveBetweenHandler(),
                // Add more handlers here as needed
            };

            foreach (var group in descriptor.GetMembersWithValidators())
            {
                string propertyName = group.Key;
                var propertyInfo = type.GetProperty(propertyName);
                if (propertyInfo == null) continue;

                var propertySchema = new JsonSchemaProperty
                {
                    Type = GetJsonType(propertyInfo.PropertyType)
                };

                foreach (var (v, options) in group)
                {
                    bool handled = false;
                    foreach (var handler in handlers)
                    {
                        if (handler.CanHandle(v))
                        {
                            handler.Apply(v, propertyInfo, propertySchema);
                            handled = true;
                            break;
                        }
                    }
                    if (handled) continue;
                }
                schema.Properties[propertyName] = propertySchema;
            }
            // Collect required properties
            foreach (var kvp in schema.Properties)
            {
                if (kvp.Value.IsRequired)
                {
                    schema.Required.Add(kvp.Key);
                }
            }
            return schema;
        }

        public string ConvertToJson<T>(IValidator<T> validator)
        {
            var schema = ConvertTo(validator);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            return JsonSerializer.Serialize(schema, options);
        }

        private string GetJsonType(Type type)
        {
            if (type == typeof(string)) return "string";
            if (type == typeof(int) || type == typeof(long) || type == typeof(short)) return "integer";
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal)) return "number";
            if (type == typeof(bool)) return "boolean";
            // Add more as needed
            return "string";
        }
    }
} 