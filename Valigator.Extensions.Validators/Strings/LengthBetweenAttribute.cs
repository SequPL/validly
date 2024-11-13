using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if a string length is within a specified range (inclusive)
/// </summary>
[Validator]
[ValidatorDescription("must be between {0} and {1}")]
[AttributeUsage(AttributeTargets.Property)]
public class LengthBetweenAttribute : Attribute
{
	private readonly int _min;
	private readonly int _max;

	/// <param name="min">The minimum allowed value.</param>
	/// <param name="max">The maximum allowed value.</param>
	public LengthBetweenAttribute(int min, int max)
	{
		_min = min;
		_max = max;
	}

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid(string? value)
	{
		if (value is not null && (value.Length < _min || value.Length > _max))
		{
			return new ValidationMessage(
				"Length must be between {0} and {1}.",
				"Valigator.Validations.LengthBetween",
				_min,
				_max
			);
		}

		return null;
	}
}
