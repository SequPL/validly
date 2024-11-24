using System.Runtime.CompilerServices;
using Validly.Validators;

namespace Validly.Extensions.Validators.Common;

/// <summary>
/// Validator that ensures a value is a non-empty string
/// </summary>
[Validator]
[ValidatorDescription("non-empty value required")]
[AttributeUsage(AttributeTargets.Property)]
public class NotEmptyAttribute : Attribute
{
	private static readonly ValidationMessage NotEmptyMessage =
		new("A non-empty value is required.", "Validly.Validations.NotEmpty");

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ValidationMessage? IsValid<T>(ICollection<T>? value)
	{
		if (value is { Count: 0 })
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ValidationMessage? IsValid<T>(IEnumerable<T>? value)
	{
		if (value is null)
		{
			return null;
		}

		if (value is ICollection<T> collection)
		{
			return IsValid(collection);
		}

		return !value.Any() ? NotEmptyMessage : null;
	}
}
