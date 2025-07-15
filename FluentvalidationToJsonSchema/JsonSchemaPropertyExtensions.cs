namespace FluentvalidationToJsonSchema
{
    public static class JsonSchemaPropertyExtensions
    {
        public static void ApplyMinLength(this JsonSchemaProperty prop, int min)
        {
            prop.MinLength = prop.MinLength.HasValue ? Math.Max(prop.MinLength.Value, min) : min;
        }

        public static void ApplyMaxLength(this JsonSchemaProperty prop, int max)
        {
            prop.MaxLength = prop.MaxLength.HasValue ? Math.Min(prop.MaxLength.Value, max) : max;
        }

        public static void ApplyMinimum(this JsonSchemaProperty prop, int min)
        {
            prop.Minimum = prop.Minimum.HasValue ? Math.Max(prop.Minimum.Value, min) : min;
        }

        public static void ApplyMaximum(this JsonSchemaProperty prop, int max)
        {
            prop.Maximum = prop.Maximum.HasValue ? Math.Min(prop.Maximum.Value, max) : max;
        }
    }
} 