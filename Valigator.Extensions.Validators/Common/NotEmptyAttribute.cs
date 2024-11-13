using System.Collections;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Common;

/// <summary>
/// Validator that ensures a value is a non-empty string
/// </summary>
[Validator]
[ValidatorDescription("non-empty value required")]
[AttributeUsage(AttributeTargets.Property)]
public class NotEmptyAttribute : Attribute
{
	private static readonly ValidationMessage NotEmptyMessage =
		new("A non-empty value is required.", "Valigator.Validations.NotEmpty");

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid(string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return NotEmptyMessage;
		}

		return null;
	}

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
