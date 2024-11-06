using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// TODO: Add description
/// </summary>
[Validator]
public class MaxLengthValidator : Validator
{
	private readonly int _maxLength;

	/// <param name="maxLength"></param>
	[ValidatorDescription("must be no more than {0} characters long")]
	public MaxLengthValidator(int maxLength)
	{
		_maxLength = maxLength;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is string strValue && strValue.Length > _maxLength)
		{
			yield return new ValidationMessage(
				"Must be no more than {0} characters long.",
				"Valigator.Validations.MaxLength",
				value
			);
		}
	}
}

// [AttributeUsage(AttributeTargets.Property)]
// public class MinLengthAttribute(int minLength) : ValidatorAttribute(typeof(MinLengthValidator)) { }
