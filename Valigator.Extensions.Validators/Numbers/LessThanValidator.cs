using Valigator.Validators;

namespace Valigator.Extensions.Validators.Numbers;

/// <summary>
/// Validator that checks if a value is less than a specified maximum
/// </summary>
[Validator]
[ValidationAttribute(typeof(LessThanAttribute))]
public class LessThanValidator : Validator
{
	private readonly decimal _max;

	/// <param name="max">The maximum value.</param>
	[ValidatorDescription("must be less than {0}")]
	public LessThanValidator(decimal max)
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

		if (decimalValue >= _max)
		{
			yield return new ValidationMessage("Must be less than {0}.", "Valigator.Validations.LessThan", value);
		}
	}
}

#pragma warning disable CS1591 // Comment is on source-generated part
public partial class LessThanAttribute;
#pragma warning restore CS1591
