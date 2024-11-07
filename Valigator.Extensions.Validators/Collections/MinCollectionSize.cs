using System.Collections;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Collections;

/// <summary>
/// Validator that checks if a collection has at least a specified minimum size.
/// </summary>
[Validator]
[ValidationAttribute(typeof(MinCollectionSizeAttribute))]
[ValidatorDescription("must contain at least {0} items")]
public class MinCollectionSizeValidator : Validator
{
	private readonly int _minSize;

	/// <param name="minSize">The minimum required number of items.</param>
	public MinCollectionSizeValidator(int minSize)
	{
		_minSize = minSize;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (
			(value is ICollection collection && collection.Count < _minSize)
			|| (value is IEnumerable enumerable && enumerable.Cast<object>().Count() < _minSize)
		)
		{
			yield return new ValidationMessage(
				$"Must contain at least {_minSize} items.",
				"Valigator.Validations.MinCollectionSize",
				value
			);
		}
	}
}

#pragma warning disable CS1591
public partial class MinCollectionSizeAttribute;
#pragma warning restore CS1591
