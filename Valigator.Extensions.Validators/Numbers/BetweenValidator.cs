using Valigator.Validators;

namespace Valigator.Extensions.Validators.Numbers;

/// <summary>
/// Validator that checks if a numeric value is within a specified range (inclusive)
/// </summary>
[Validator]
[ValidationAttribute(typeof(BetweenAttribute))]
[ValidatorDescription("must be between {0} and {1}")]
public class BetweenValidator : Validator
{
	private readonly decimal _min;
	private readonly decimal _max;

	/// <param name="min">The minimum allowed value.</param>
	/// <param name="max">The maximum allowed value.</param>
	public BetweenValidator(decimal min, decimal max)
	{
		_min = min;
		_max = max;
	}

	/// <param name="min">The minimum allowed value.</param>
	/// <param name="max">The maximum allowed value.</param>
	public BetweenValidator(int min, int max)
	{
		_min = min;
		_max = max;
	}

	/// <param name="min">The minimum allowed value.</param>
	/// <param name="max">The maximum allowed value.</param>
	public BetweenValidator(double min, double max)
	{
		_min = (decimal)min;
		_max = (decimal)max;
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

#pragma warning disable CS1591 // Comment is on source-generated part
public partial class BetweenAttribute;
#pragma warning restore CS1591
