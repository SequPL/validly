using Valigator.Validators;

namespace Valigator.Extensions.Validators.Numbers;

/// <summary>
/// Validator that checks if a value is less than a specified maximum
/// </summary>
[Validator]
[ValidatorDescription("must be less than {0}")]
[AttributeUsage(AttributeTargets.Property)]
public class LessThanAttribute : Attribute
{
	private readonly decimal _max;

	/// <param name="max">The maximum value.</param>
	public LessThanAttribute(decimal max)
	{
		_max = max;
	}

	/// <param name="max">The maximum value.</param>
	public LessThanAttribute(int max)
	{
		_max = max;
	}

	/// <param name="max">The maximum value.</param>
	public LessThanAttribute(double max)
	{
		_max = (decimal)max;
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

		if (decimalValue >= _max)
		{
			return new ValidationMessage("Must be less than {0}.", "Valigator.Validations.LessThan", value);
		}

		return null;
	}
}
