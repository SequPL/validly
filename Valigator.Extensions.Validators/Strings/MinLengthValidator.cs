using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if the length of a string is at least a specified minimum length.
/// </summary>
[Validator]
[ValidationAttribute(typeof(MinLengthAttribute))]
public class MinLengthValidator : Validator
{
	private readonly int _minLength;

	/// <param name="minLength"></param>
	[ValidatorDescription("must be at least {0} characters long")]
	public MinLengthValidator(int minLength)
	{
		_minLength = minLength;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is string strValue && strValue.Length < _minLength)
		{
			yield return new ValidationMessage(
				"Must be at least {0} characters long.",
				"Valigator.Validations.MinLength",
				value
			);
		}
	}
}

#pragma warning disable CS1591 // Comment is on source-generated part
public partial class MinLengthAttribute;
#pragma warning restore CS1591
