using Valigator.Validators;

namespace Valigator.Extensions.Validators.Enums;

/// <summary>
/// Validator that ensures a value is within enum defined values
/// </summary>
[Validator]
[ValidatorDescription("must be one of the defined enum members")]
[AttributeUsage(AttributeTargets.Property)]
public class InEnumAttribute : Attribute
{
	private static readonly ValidationMessage ValidationMessage =
		new("Must be one of the defined enum members.", "Valigator.Validations.InEnum");

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValidationMessage? IsValid<TEnum>(TEnum value)
		where TEnum : struct, Enum
	{
		if (!Enum.IsDefined(typeof(TEnum), value))
		{
			return ValidationMessage;
		}

		return null;
	}
}
