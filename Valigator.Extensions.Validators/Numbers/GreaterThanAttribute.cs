using Valigator.Validators;

namespace Valigator.Extensions.Validators.Numbers;

/// <summary>
/// Validator that checks if a value is greater than a specified minimum
/// </summary>
[Validator]
[ValidatorDescription("must be greater than {0}")]
[AttributeUsage(AttributeTargets.Property)]
public class GreaterThanAttribute : Attribute
{
	private readonly decimal _min;

	/// <param name="min">The minimum value.</param>
	public GreaterThanAttribute(decimal min)
	{
		_min = min;
	}

	/// <param name="min">The minimum value.</param>
	public GreaterThanAttribute(int min)
	{
		_min = min;
	}

	/// <param name="min">The minimum value.</param>
	public GreaterThanAttribute(double min)
	{
		_min = (decimal)min;
	}

	// /// <param name="expression">Expression used to access a property with the value.</param>
	// public GreaterThanValidator([AsExpression] string expression)
	// {
	// 	throw new NotImplementedException();
	// }

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

		decimal decimalValue = Convert.ToDecimal(value);

		if (decimalValue <= _min)
		{
			yield return new ValidationMessage("Must be greater than {0}.", "Valigator.Validations.GreaterThan", value);
		}
	}
}
