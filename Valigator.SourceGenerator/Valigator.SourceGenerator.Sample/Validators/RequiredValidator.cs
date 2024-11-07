using System.Collections.Generic;
using Valigator.Validators;

namespace Valigator.SourceGenerator.Sample.Validators;

/// <summary></summary>
/// <param name="allowEmptyStrings">If true, empty strings are considered valid values.</param>
[Validator]
[ValidatorDescription("is required")]
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
