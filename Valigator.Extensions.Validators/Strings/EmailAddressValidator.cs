using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// TODO: Add description
/// </summary>
[ValidatorDescription("must be a valid email address")]
[Validator]
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

// [Validator(typeof(EmailAddressValidator)), AttributeUsage(AttributeTargets.Property)]
// public class EmailAddressAttribute();
