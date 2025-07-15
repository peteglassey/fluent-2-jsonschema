namespace FluentvalidationToJsonSchema
{
    public class JsonSchemaProperty
    {
        public string Type { get; set; }= "";
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public int? Minimum { get; set; }
        public int? Maximum { get; set; }
        public string? Format { get; set; } 
        public string? Pattern { get; set; }
        public object[] Enum { get; set; }
        public bool IsRequired { get; set; } = false;
    }
} 