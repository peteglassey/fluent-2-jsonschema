using FluentValidation;
using FluentvalidationToJsonSchema;
using Xunit;
using System.Text.Json;
using System;

namespace UnitTests
{
    public class FluentValidatorToJsonSchemaConverterTests
    {
        public class TestModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }
        }

        public class TestModelValidator : AbstractValidator<TestModel>
        {
            public TestModelValidator()
            {
                RuleFor(x => x.Name).NotEmpty().Length(2, 10);
                RuleFor(x => x.Age).InclusiveBetween(18, 99);
                RuleFor(x => x.Email).EmailAddress();
            }
        }

        public class ExclusiveBetweenModel
        {
            public int Value { get; set; }
        }

        public class ExclusiveBetweenModelValidator : AbstractValidator<ExclusiveBetweenModel>
        {
            public ExclusiveBetweenModelValidator()
            {
                RuleFor(x => x.Value).ExclusiveBetween(10, 20);
            }
        }

        public enum TestEnum { A, B, C }
        public class EnumModel { public TestEnum Value { get; set; } }
        public class MaxMinLengthModel { public string Text { get; set; } }
        public class GreaterLessModel { public int Number { get; set; } }
        public class LengthModel { public string Text { get; set; } }

        public class EnumModelValidator : AbstractValidator<EnumModel>
        {
            public EnumModelValidator() { RuleFor(x => x.Value).IsInEnum(); }
        }
        public class MaxMinLengthModelValidator : AbstractValidator<MaxMinLengthModel>
        {
            public MaxMinLengthModelValidator() { RuleFor(x => x.Text).MaximumLength(5).MinimumLength(2); }
        }
        public class GreaterLessModelValidator : AbstractValidator<GreaterLessModel>
        {
            public GreaterLessModelValidator()
            {
                RuleFor(x => x.Number).GreaterThan(10);
                RuleFor(x => x.Number).GreaterThanOrEqualTo(20);
                RuleFor(x => x.Number).LessThan(100);
                RuleFor(x => x.Number).LessThanOrEqualTo(200);
            }
        }
        public class LengthModelValidator : AbstractValidator<LengthModel>
        {
            public LengthModelValidator() { RuleFor(x => x.Text).Length(3, 8); }
        }

        [Fact]
        public void Converts_NotEmpty_And_Length_To_JsonSchema()
        {
            var validator = new TestModelValidator();
            var converter = new FluentValidatorToJsonSchemaConverter();
            var schema = converter.ConvertTo(validator);

            Assert.True(schema.Properties.ContainsKey("Name"));
            var nameProp = schema.Properties["Name"];
            Assert.Equal("string", nameProp.Type);
            Assert.Equal(2, nameProp.MinLength);
            Assert.Equal(10, nameProp.MaxLength);
            Assert.Contains("Name", schema.Required);
        }

        [Fact]
        public void Converts_InclusiveBetween_To_JsonSchema()
        {
            var validator = new TestModelValidator();
            var converter = new FluentValidatorToJsonSchemaConverter();
            var schema = converter.ConvertTo(validator);

            Assert.True(schema.Properties.ContainsKey("Age"));
            var ageProp = schema.Properties["Age"];
            Assert.Equal("integer", ageProp.Type);
            Assert.Equal(18, ageProp.Minimum);
            Assert.Equal(99, ageProp.Maximum);
        }

        [Fact]
        public void Converts_EmailAddress_To_JsonSchema()
        {
            var validator = new TestModelValidator();
            var converter = new FluentValidatorToJsonSchemaConverter();
            var schema = converter.ConvertTo(validator);

            Assert.True(schema.Properties.ContainsKey("Email"));
            var emailProp = schema.Properties["Email"];
            Assert.Equal("string", emailProp.Type);
            Assert.Equal("email", emailProp.Format);
        }

        [Fact]
        public void Converts_ExclusiveBetween_To_JsonSchema()
        {
            var validator = new ExclusiveBetweenModelValidator();
            var converter = new FluentValidatorToJsonSchemaConverter();
            var schema = converter.ConvertTo(validator);

            Assert.True(schema.Properties.ContainsKey("Value"));
            var valueProp = schema.Properties["Value"];
            Assert.Equal("integer", valueProp.Type);
            Assert.Equal(11, valueProp.Minimum); // 10 + 1
            Assert.Equal(19, valueProp.Maximum); // 20 - 1
        }

        [Fact]
        public void Converts_MaxMinLength_To_JsonSchema()
        {
            var validator = new MaxMinLengthModelValidator();
            var converter = new FluentValidatorToJsonSchemaConverter();
            var schema = converter.ConvertTo(validator);
            var prop = schema.Properties["Text"];
            Assert.Equal(2, prop.MinLength);
            Assert.Equal(5, prop.MaxLength);
        }

        [Fact]
        public void Converts_GreaterLess_To_JsonSchema()
        {
            var validator = new GreaterLessModelValidator();
            var converter = new FluentValidatorToJsonSchemaConverter();
            var schema = converter.ConvertTo(validator);
            var prop = schema.Properties["Number"];
            Assert.Equal(20, prop.Minimum); // GreaterThan(10) + GreaterThanOrEqualTo(20) => 20 wins
            Assert.Equal(99, prop.Maximum); // LessThan(100) => 99, LessThanOrEqualTo(200) => 200, so 99 wins
        }

        [Fact]
        public void Converts_Length_To_JsonSchema()
        {
            var validator = new LengthModelValidator();
            var converter = new FluentValidatorToJsonSchemaConverter();
            var schema = converter.ConvertTo(validator);
            var prop = schema.Properties["Text"];
            Assert.Equal(3, prop.MinLength);
            Assert.Equal(8, prop.MaxLength);
        }

        [Fact]
        public void Converts_IsInEnum_To_JsonSchema()
        {
            var validator = new EnumModelValidator();
            var converter = new FluentValidatorToJsonSchemaConverter();
            var schema = converter.ConvertTo(validator);
            var prop = schema.Properties["Value"];
            Assert.NotNull(prop.Enum);
            Assert.Contains(TestEnum.A, prop.Enum);
            Assert.Contains(TestEnum.B, prop.Enum);
            Assert.Contains(TestEnum.C, prop.Enum);
        }

        [Fact]
        public void Serializes_To_Expected_Json()
        {
            var validator = new TestModelValidator();
            var converter = new FluentValidatorToJsonSchemaConverter();
            var schema = converter.ConvertTo(validator);
            var json = JsonSerializer.Serialize(schema, new JsonSerializerOptions {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
            System.Diagnostics.Debug.WriteLine(json);
            Console.WriteLine(json);

            Assert.Contains("\"minLength\": 2", json);
            Assert.Contains("\"maxLength\": 10", json);
            Assert.Contains("\"minimum\": 18", json);
            Assert.Contains("\"maximum\": 99", json);
            Assert.Contains("\"format\": \"email\"", json);
        }
    }
} 