using System.Collections;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Collections;

/// <summary>
/// Validator that checks if a collection does not exceed a specified maximum size.
/// </summary>
[Validator]
[ValidationAttribute(typeof(MaxCollectionSizeAttribute))]
[ValidatorDescription("must contain no more than {0} items")]
public class MaxCollectionSizeValidator : Validator
{
	private readonly int _maxSize;

	/// <param name="maxSize">The maximum allowed number of items.</param>
	public MaxCollectionSizeValidator(int maxSize)
	{
		_maxSize = maxSize;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (
			(value is ICollection collection && collection.Count > _maxSize)
			|| (value is IEnumerable enumerable && enumerable.Cast<object>().Count() > _maxSize)
		)
		{
			yield return new ValidationMessage(
				$"Must contain no more than {_maxSize} items.",
				"Valigator.Validations.MaxCollectionSize",
				value
			);
		}
	}
}

#pragma warning disable CS1591
public partial class MaxCollectionSizeAttribute;
#pragma warning restore CS1591
