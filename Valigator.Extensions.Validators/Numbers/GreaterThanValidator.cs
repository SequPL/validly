using Valigator.Validators;

namespace Valigator.Extensions.Validators.Numbers;

/// <summary>
/// Validator that checks if a value is greater than a specified minimum
/// </summary>
[Validator]
[ValidationAttribute(typeof(GreaterThanAttribute))]
public class GreaterThanValidator : Validator
{
	private readonly decimal _min;

	/// <param name="min"></param>
	[ValidatorDescription("must be greater than {0}")]
	public GreaterThanValidator(decimal min)
	{
		_min = min;
	}

	/// <param name="expression">Expression used to access a property with the value.</param>
	[ValidatorDescription("must be greater than {0}")]
	public GreaterThanValidator([AsExpression] string expression)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
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

#pragma warning disable CS1591 // Comment is on source-generated part
public partial class GreaterThanAttribute;
#pragma warning restore CS1591
