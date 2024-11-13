using System.Collections;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Collections;

/// <summary>
/// Validator that checks if a collection is not empty.
/// </summary>
[Validator]
[ValidatorDescription("must not be an empty collection")]
[AttributeUsage(AttributeTargets.Property)]
public class NotEmptyCollectionAttribute : Attribute
{
	private static readonly ValidationMessage NotEmptyMessage =
		new("Must not be an empty collection.", "Valigator.Validations.NotEmptyCollection");

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid<T>(IEnumerable<T>? value)
	{
		if (value is ICollection { Count: 0 } || (value is IEnumerable enumerable && !enumerable.Cast<object>().Any()))
		{
			return NotEmptyMessage;
		}

		return null;
	}
}
