using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if a value is a valid email address format.
/// This validator ensures that the input contains exactly one '@' character, with characters on both sides.
/// Typically used to validate that a property contains a properly formatted email address.
/// </summary>
[Validator]
[ValidationAttribute(typeof(EmailAddressAttribute))]
[ValidatorDescription("must be a valid email address")]
public class EmailAddressValidator : Validator
{
	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (!Valid(value))
		{
			yield return new ValidationMessage("Must be a valid email address", "Valigator.Validations.EmailAddress");
		}
	}

	private static bool Valid(object? value)
	{
		if (value == null)
		{
			return true;
		}

		if (!(value is string valueAsString))
		{
			return false;
		}

		// only return true if there is only 1 '@' character
		// and it is neither the first nor the last character
		int index = valueAsString.IndexOf('@');

		return index > 0 && index != valueAsString.Length - 1 && index == valueAsString.LastIndexOf('@');
	}
}

#pragma warning disable CS1591 // Comment is on source-generated part
public partial class EmailAddressAttribute;
#pragma warning restore CS1591
