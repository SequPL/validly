using System.Collections;
using Valigator.Extensions.Validators.Common;
using Valigator.Extensions.Validators.Numbers;
using Valigator.Extensions.Validators.Strings;
using Valigator.Validators;

namespace Valigator.SourceGenerator.Sample;

[Validator]
[ValidationAttribute(typeof(DemoContextAttribute))]
public class DemoContextValidator : ContextValidator
{
	public override IEnumerable<ValidationMessage> IsValid(object? value, ValidationContext context)
	{
		throw new NotImplementedException();
	}
}

public partial class DemoContextAttribute;

[Validatable]
public partial class CreateUserRequest
{
	[Required]
	[MinLength(3)]
	[MaxLength(100)]
	public string Name { get; set; }

	[Required]
	[EmailAddress]
	[CustomValidation]
	public string Email { get; set; }

	[Between(18, 99)]
	[DemoContext]
	public int Age { get; set; }

	[GreaterThan(12)]
	public int Age2 { get; set; }

	// IEnumerable<ValidationMessage> ICreateUserRequestCustomValidation.ValidateEmail()
	// {
	// 	return [];
	// }

	IEnumerable<ValidationMessage> ICreateUserRequestCustomValidation.ValidateEmail(ValidationContext context)
	{
		if (Email.Contains("localhost"))
		{
			yield return new ValidationMessage(
				"Email cannot contain 'localhost'.",
				"Valigator.Validations.EmailAddress"
			);
		}
	}

	private ValidationResult? BeforeValidate()
	{
		// if (DateTime.Now.Hour > 16)
		// {
		// 	yield return new ValidationMessage(
		// 		"Validation can only be performed before 5 PM.",
		// 		"Validations.DumbBeforeValidateExample"
		// 	);
		// }
		return null;
	}

	private ValidationResult AfterValidate(ValidationResult result)
	{
		return result;
	}
}
