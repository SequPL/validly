using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

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
	public IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is string strValue && string.IsNullOrWhiteSpace(strValue))
		{
			yield return NotEmptyMessage;
		}
	}
}
