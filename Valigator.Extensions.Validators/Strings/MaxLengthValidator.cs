using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if the length of a string does not exceed a specified maximum length.
/// This validator is typically used to ensure that a property value, if it is a string, has a length within the allowed limit.
/// </summary>
[Validator]
[ValidationAttribute(typeof(MaxLengthAttribute))]
public class MaxLengthValidator : Validator
{
	private readonly int _maxLength;

	/// <param name="maxLength">The maximum allowed length for the string value.</param>
	[ValidatorDescription("must be no more than {0} characters long")]
	public MaxLengthValidator(int maxLength)
	{
		_maxLength = maxLength;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is string strValue && strValue.Length > _maxLength)
		{
			yield return new ValidationMessage(
				"Must be no more than {0} characters long.",
				"Valigator.Validations.MaxLength",
				value
			);
		}
	}
}

#pragma warning disable CS1591 // Comment is on source-generated part
public partial class MinLengthAttribute;
#pragma warning restore CS1591
