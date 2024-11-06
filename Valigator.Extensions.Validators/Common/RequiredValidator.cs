using Valigator.Validators;

namespace Valigator.Extensions.Validators.Common;

/// <summary>
/// TODO: Add description
/// </summary>
/// <param name="allowEmptyStrings">If true, empty strings are considered valid values.</param>
[ValidatorDescription("is required")]
[Validator]
public class RequiredValidator(bool allowEmptyStrings = false) : Validator
{
	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (
			value is null
			|| !(allowEmptyStrings || value is not string stringValue || !string.IsNullOrWhiteSpace(stringValue))
		)
		{
			yield return new ValidationMessage("Required.", "Valigator.Validations.Required");
		}
	}
}

// [AttributeUsage(AttributeTargets.Property)]
// public class RequiredAttribute(bool allowEmptyStrings = false) : Attribute;
