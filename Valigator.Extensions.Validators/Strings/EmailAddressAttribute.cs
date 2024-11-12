using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if a value is a valid email address format.
/// This validator ensures that the input contains exactly one '@' character, with characters on both sides.
/// Typically used to validate that a property contains a properly formatted email address.
/// </summary>
[Validator]
[ValidatorDescription("must be a valid email address")]
[AttributeUsage(AttributeTargets.Property)]
public class EmailAddressAttribute : Attribute
{
	private static readonly ValidationMessage ValidationMessage =
		new("Must be a valid email address", "Valigator.Validations.EmailAddress");

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (!Valid(value))
		{
			yield return ValidationMessage;
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
