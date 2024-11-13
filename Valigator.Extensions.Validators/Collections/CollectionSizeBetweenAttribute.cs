using System.Collections;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Collections;

/// <summary>
/// Validator that checks if a collection size is within a specified range (inclusive).
/// </summary>
[Validator]
[AttributeUsage(AttributeTargets.Property)]
public class CollectionSizeBetweenAttribute : Attribute
{
	private readonly int _minSize;
	private readonly int _maxSize;

	/// <param name="minSize">The minimum number of items allowed.</param>
	/// <param name="maxSize">The maximum number of items allowed.</param>
	[ValidatorDescription("must contain between {0} and {1} items")]
	public CollectionSizeBetweenAttribute(int minSize, int maxSize)
	{
		_minSize = minSize;
		_maxSize = maxSize;
	}

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid<T>(IEnumerable<T>? value)
	{
		if (value is ICollection collection && (collection.Count < _minSize || collection.Count > _maxSize))
		{
			return CreateErrorValidationMessage();
		}

		if (value is IEnumerable enumerable)
		{
			int count = enumerable.Cast<object>().Count();

			if (count < _minSize || count > _maxSize)
			{
				return CreateErrorValidationMessage();
			}
		}

		return null;
	}

	private ValidationMessage CreateErrorValidationMessage()
	{
		return new ValidationMessage(
			"Must contain between {0} and {1} items.",
			"Valigator.Validations.CollectionSizeBetween",
			_minSize,
			_maxSize
		);
	}
}
