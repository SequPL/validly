using Valigator.Extensions.Validators.Common;
using Valigator.Extensions.Validators.Numbers;
using Valigator.Extensions.Validators.Strings;
using Valigator.Validators;

namespace Valigator.SourceGenerator.Sample;

[Validator]
public class DemoContextValidator : ContextValidator
{
	public override IEnumerable<ValidationMessage> IsValid(object? value, ValidationContext context)
	{
		throw new NotImplementedException();
	}
}

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

	[Range(18, 99)]
	[DemoContext]
	public int Age { get; set; }

	[GreaterThan(nameof(Age))]
	public int Age2 { get; set; }

	public IEnumerable<ValidationMessage> ValidateEmail()
	{
		if (Email.Contains("localhost"))
		{
			yield return new ValidationMessage("Email must not contain 'localhost'.", "Valigator.Validations.Email");
		}
	}
}
