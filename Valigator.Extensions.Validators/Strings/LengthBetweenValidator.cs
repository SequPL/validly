using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if a string length is within a specified range (inclusive)
/// </summary>
[Validator]
[ValidationAttribute(typeof(LengthBetweenAttribute))]
[ValidatorDescription("must be between {0} and {1}")]
public class LengthBetweenValidator : Validator
{
	private readonly int _min;
	private readonly int _max;

	/// <param name="min">The minimum allowed value.</param>
	/// <param name="max">The maximum allowed value.</param>
	public LengthBetweenValidator(int min, int max)
	{
		_min = min;
		_max = max;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is not string stringValue)
		{
			yield break;
		}

		if (stringValue.Length < _min || stringValue.Length > _max)
		{
			yield return new ValidationMessage(
				"Length must be between {0} and {1}.",
				"Valigator.Validations.LengthBetween",
				_min,
				_max
			);
		}
	}
}

#pragma warning disable CS1591 // Comment is on source-generated part
public partial class LengthBetweenAttribute;
#pragma warning restore CS1591
