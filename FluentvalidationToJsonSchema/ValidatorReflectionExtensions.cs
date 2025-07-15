namespace FluentvalidationToJsonSchema
{
    public static class ValidatorReflectionExtensions
    {
        public static object? GetValueToCompare(this object validator)
        {
            var prop = validator.GetType().GetProperty(nameof(FluentValidation.Validators.IComparisonValidator.ValueToCompare));
            return prop?.GetValue(validator);
        }

        public static object? GetFrom(this object validator)
        {
            var prop = validator.GetType().GetProperty(nameof(FluentValidation.Validators.IBetweenValidator.From));
            return prop?.GetValue(validator);
        }

        public static object? GetTo(this object validator)
        {
            var prop = validator.GetType().GetProperty(nameof(FluentValidation.Validators.IBetweenValidator.To));
            return prop?.GetValue(validator);
        }
    }
} 