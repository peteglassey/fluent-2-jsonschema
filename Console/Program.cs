using System;
using FluentValidation;
using FluentvalidationToJsonSchema;

public class Person
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(x => x.LastName).MaximumLength(50)
        .NotEmpty()
        .Length(2, 50);

        RuleFor(x => x.Age)
        .InclusiveBetween(0, 120);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var validator = new PersonValidator();
        var converter = new FluentValidatorToJsonSchemaConverter();
        var json = converter.ConvertToJson(validator);
        Console.WriteLine(json);
    }
}
