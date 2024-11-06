using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// TODO: Add description
/// </summary>
[Validator]
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

// [AttributeUsage(AttributeTargets.Property)]
// public class MinLengthAttribute(int minLength) : ValidatorAttribute(typeof(MinLengthValidator)) { }
