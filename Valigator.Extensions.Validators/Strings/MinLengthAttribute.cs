using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if the length of a string is at least a specified minimum length.
/// </summary>
[Validator]
[AttributeUsage(AttributeTargets.Property)]
public class MinLengthAttribute : Attribute
{
	private readonly int _minLength;

	/// <param name="minLength"></param>
	[ValidatorDescription("must be at least {0} characters long")]
	public MinLengthAttribute(int minLength)
	{
		_minLength = minLength;
	}

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid(string? value)
	{
		if (value is not null && value.Length < _minLength)
		{
			return new ValidationMessage(
				"Must be at least {0} characters long.",
				"Valigator.Validations.MinLength",
				value
			);
		}

		return null;
	}
}
