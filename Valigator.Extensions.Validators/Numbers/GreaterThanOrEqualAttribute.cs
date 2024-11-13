using Valigator.Validators;

namespace Valigator.Extensions.Validators.Numbers;

/// <summary>
/// Validator that checks if a value is greater than or equal to a specified minimum
/// </summary>
[Validator]
[ValidatorDescription("must be greater than or equal to {0}")]
[AttributeUsage(AttributeTargets.Property)]
public class GreaterThanOrEqualAttribute : Attribute
{
	private readonly decimal _min;

	/// <param name="min">The minimum value.</param>
	public GreaterThanOrEqualAttribute(decimal min)
	{
		_min = min;
	}

	/// <param name="min">The minimum value.</param>
	public GreaterThanOrEqualAttribute(int min)
	{
		_min = min;
	}

	/// <param name="min">The minimum value.</param>
	public GreaterThanOrEqualAttribute(double min)
	{
		_min = (decimal)min;
	}

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid(object? value)
	{
		if (value is null)
		{
			return null;
		}

		decimal decimalValue = Convert.ToDecimal(value);

		if (decimalValue < _min)
		{
			return new ValidationMessage(
				"Must be greater than or equal to {0}.",
				"Valigator.Validations.GreaterThanOrEqual",
				value
			);
		}

		return null;
	}
}
