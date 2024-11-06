using Valigator.Validators;

namespace Valigator.Extensions.Validators.Numbers;

/// <summary>
/// Validator that checks if a value is less than or equal to a specified maximum.
/// </summary>
[Validator]
public class LessThanOrEqualValidator : Validator
{
	private readonly decimal _max;

	/// <param name="max">The maximum value.</param>
	[ValidatorDescription("must be less than or equal to {0}")]
	public LessThanOrEqualValidator(decimal max)
	{
		_max = max;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is null)
		{
			yield break;
		}

		decimal decimalValue = Convert.ToDecimal(value);

		if (decimalValue > _max)
		{
			yield return new ValidationMessage(
				"Must be less than or equal to {0}.",
				"Valigator.Validations.LessThanOrEqual",
				value
			);
		}
	}
}
