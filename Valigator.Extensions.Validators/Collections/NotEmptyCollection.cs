using System.Collections;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Collections;

/// <summary>
/// Validator that checks if a collection is not empty.
/// </summary>
[Validator]
[ValidationAttribute(typeof(NotEmptyCollectionAttribute))]
[ValidatorDescription("must not be an empty collection")]
public class NotEmptyCollectionValidator : Validator
{
	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is ICollection { Count: 0 } || (value is IEnumerable enumerable && !enumerable.Cast<object>().Any()))
		{
			yield return new ValidationMessage(
				"Must not be an empty collection.",
				"Valigator.Validations.NotEmptyCollection"
			);
		}
	}
}

#pragma warning disable CS1591
public partial class NotEmptyCollectionAttribute;
#pragma warning restore CS1591
