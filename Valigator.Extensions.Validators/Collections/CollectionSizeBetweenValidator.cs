using System.Collections;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Collections;

/// <summary>
/// Validator that checks if a collection size is within a specified range (inclusive).
/// </summary>
[Validator]
[ValidationAttribute(typeof(CollectionSizeBetweenAttribute))]
[ValidatorDescription("must contain between {0} and {1} items")]
public class CollectionSizeBetweenValidator : Validator
{
	private readonly int _minSize;
	private readonly int _maxSize;

	/// <param name="minSize">The minimum number of items allowed.</param>
	/// <param name="maxSize">The maximum number of items allowed.</param>
	public CollectionSizeBetweenValidator(int minSize, int maxSize)
	{
		_minSize = minSize;
		_maxSize = maxSize;
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is ICollection collection && (collection.Count < _minSize || collection.Count > _maxSize))
		{
			yield return CreateErrorValidationMessage(value);
		}

		if (value is IEnumerable enumerable)
		{
			int count = enumerable.Cast<object>().Count();

			if (count < _minSize || count > _maxSize)
			{
				yield return CreateErrorValidationMessage(value);
			}
		}
	}

	private ValidationMessage CreateErrorValidationMessage(object value)
	{
		return new ValidationMessage(
			$"Must contain between {_minSize} and {_maxSize} items.",
			"Valigator.Validations.CollectionSizeBetween",
			value
		);
	}
}

#pragma warning disable CS1591
public partial class CollectionSizeBetweenAttribute;
#pragma warning restore CS1591
