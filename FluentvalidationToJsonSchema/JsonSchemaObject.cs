using System.Collections.Generic;

namespace FluentvalidationToJsonSchema
{
    public class JsonSchemaObject
    {
        public string Type { get; set; } = "object";
        public Dictionary<string, JsonSchemaProperty> Properties { get; set; } = new();
        public List<string> Required { get; set; } = new();
    }
} 