using System.Collections;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Collections;

/// <summary>
/// Validator that checks if a collection has at least a specified minimum size.
/// </summary>
[Validator]
[AttributeUsage(AttributeTargets.Property)]
public class MinCollectionSizeAttribute : Attribute
{
	private readonly int _minSize;

	/// <param name="minSize">The minimum required number of items.</param>
	[ValidatorDescription("must contain at least {0} items")]
	public MinCollectionSizeAttribute(int minSize)
	{
		_minSize = minSize;
	}

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid<T>(IEnumerable<T>? value)
	{
		if (
			(value is ICollection collection && collection.Count < _minSize)
			|| (value is IEnumerable enumerable && enumerable.Cast<object>().Count() < _minSize)
		)
		{
			return new ValidationMessage(
				"Must contain at least {0} items.",
				"Valigator.Validations.MinCollectionSize",
				_minSize
			);
		}

		return null;
	}
}
