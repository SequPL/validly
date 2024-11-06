using System;
using System.Collections.Generic;
using Valigator.Validators;

namespace Valigator.SourceGenerator.Sample.Validators;

/// <summary>
/// Validator that checks if a value is greater than or equal to a specified minimum
/// </summary>
[Validator]
public class GreaterThanOrEqualValidator : Validator
{
	private readonly decimal _min;

	/// <param name="min">The minimum value.</param>
	[ValidatorDescription("must be greater than or equal to {0}")]
	public GreaterThanOrEqualValidator(decimal min)
	{
		_min = min;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is null)
		{
			yield break;
		}

		decimal decimalValue = Convert.ToDecimal(value);

		if (decimalValue < _min)
		{
			yield return new ValidationMessage(
				"Must be greater than or equal to {0}.",
				"Valigator.Validations.GreaterThanOrEqual",
				value
			);
		}
	}
}
