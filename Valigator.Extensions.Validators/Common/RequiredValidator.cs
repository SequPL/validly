using Valigator.Validators;

namespace Valigator.Extensions.Validators.Common;

/// <summary>
/// Validator that ensures a value is present, enforcing it as required.
/// If the value is a string, it can optionally allow empty strings based on the configuration of <paramref name="allowEmptyStrings"/>.
/// Typically used to validate that a property has a non-null or non-empty value.
/// </summary>
/// <param name="allowEmptyStrings">If true, empty strings are considered valid values; otherwise, they are treated as missing values.</param>
[Validator]
[ValidationAttribute(typeof(RequiredAttribute))]
[ValidatorDescription("is required")]
public class RequiredValidator(bool allowEmptyStrings = false) : Validator
{
	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is null || (value is string stringValue && string.IsNullOrEmpty(stringValue) && !allowEmptyStrings))
		{
			yield return new ValidationMessage("Required.", "Valigator.Validations.Required");
		}
	}
}

#pragma warning disable CS1591 // Comment is on source-generated part
public partial class RequiredAttribute;
#pragma warning restore CS1591
