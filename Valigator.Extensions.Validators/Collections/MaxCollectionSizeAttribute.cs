using System.Collections;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Collections;

/// <summary>
/// Validator that checks if a collection does not exceed a specified maximum size.
/// </summary>
[Validator]
[AttributeUsage(AttributeTargets.Property)]
public class MaxCollectionSizeAttribute : Attribute
{
	private readonly int _maxSize;

	/// <param name="maxSize">The maximum allowed number of items.</param>
	[ValidatorDescription("must contain no more than {0} items")]
	public MaxCollectionSizeAttribute(int maxSize)
	{
		_maxSize = maxSize;
	}

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid<T>(IEnumerable<T>? value)
	{
		if (
			(value is ICollection collection && collection.Count > _maxSize)
			|| (value is IEnumerable enumerable && enumerable.Cast<object>().Count() > _maxSize)
		)
		{
			return new ValidationMessage(
				"Must contain no more than {0} items.",
				"Valigator.Validations.MaxCollectionSize",
				_maxSize
			);
		}

		return null;
	}
}

// #pragma warning disable CS1591
// public partial class MaxCollectionSizeAttribute;
// #pragma warning restore CS1591
