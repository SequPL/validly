using Valigator.Validators;

namespace Valigator.Extensions.Validators.Common;

/// <summary>
/// Validator that checks if a numeric value or string length is within a specified range (inclusive).
/// </summary>
[Validator]
public class RangeValidator : Validator
{
	private readonly decimal _min;
	private readonly decimal _max;

	/// <param name="min">The minimum allowed value.</param>
	/// <param name="max">The maximum allowed value.</param>
	[ValidatorDescription("must be between {0} and {1}")]
	public RangeValidator(decimal min, decimal max)
	{
		_min = min;
		_max = max;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
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
					"Length must be between {0} and {1}.",
					"Valigator.Validations.Range",
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
				"Valigator.Validations.Range",
				_min,
				_max
			);
		}
	}
}
