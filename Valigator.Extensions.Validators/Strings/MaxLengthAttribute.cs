using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if the length of a string does not exceed a specified maximum length.
/// This validator is typically used to ensure that a property value, if it is a string, has a length within the allowed limit.
/// </summary>
[Validator]
[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute
{
	private readonly int _maxLength;

	/// <param name="maxLength">The maximum allowed length for the string value.</param>
	[ValidatorDescription("must be no more than {0} characters long")]
	public MaxLengthAttribute(int maxLength)
	{
		_maxLength = maxLength;
	}

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid(string? value)
	{
		if (value is not null && value.Length > _maxLength)
		{
			return new ValidationMessage(
				"Must be no more than {0} characters long.",
				"Valigator.Validations.MaxLength",
				value
			);
		}

		return null;
	}
}
