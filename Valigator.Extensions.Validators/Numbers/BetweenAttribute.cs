using Valigator.Validators;

namespace Valigator.Extensions.Validators.Numbers;

/// <summary>
/// Validator that checks if a numeric value is within a specified range (inclusive)
/// </summary>
[Validator]
[ValidatorDescription("must be between {0} and {1}")]
[AttributeUsage(AttributeTargets.Property)]
public class BetweenAttribute : Attribute
{
	private readonly decimal _min;
	private readonly decimal _max;

	/// <param name="min">The minimum allowed value.</param>
	/// <param name="max">The maximum allowed value.</param>
	public BetweenAttribute(decimal min, decimal max)
	{
		_min = min;
		_max = max;
	}

	/// <param name="min">The minimum allowed value.</param>
	/// <param name="max">The maximum allowed value.</param>
	public BetweenAttribute(int min, int max)
	{
		_min = min;
		_max = max;
	}

	/// <param name="min">The minimum allowed value.</param>
	/// <param name="max">The maximum allowed value.</param>
	public BetweenAttribute(double min, double max)
	{
		_min = (decimal)min;
		_max = (decimal)max;
	}

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is null)
		{
			yield break;
		}

		if (value is string stringValue)
		{
			if (stringValue.Length < _min || stringValue.Length > _max)
			{
				yield return new ValidationMessage(
					"Value must be between {0} and {1}.",
					"Valigator.Validations.Between",
					_min,
					_max
				);
			}
		}

		decimal decimalValue = Convert.ToDecimal(value);

		if (decimalValue < _min || decimalValue > _max)
		{
			yield return new ValidationMessage(
				"Must be between {0} and {1}.",
				"Valigator.Validations.Between",
				_min,
				_max
			);
		}
	}
}
