using Valigator.Validators;

namespace Valigator.Extensions.Validators.Enums;

/// <summary>
/// Validator that ensures a value is within enum defined values
/// </summary>
[Validator]
[ValidatorDescription("value must be within enum defined values")]
[AttributeUsage(AttributeTargets.Property)]
public class DefinedAttribute : Attribute
{

  /// <summary>
  /// Validate the value
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public ValidationMessage? IsValid<T>(T value)
  {
    if (value is IEnumerable<T> enumerable && !Enum.IsDefined(typeof(T), value))
    {
      return new ValidationMessage(
        "Value must be within enum defined values.",
        "Valigator.Validations.Defined"
      );
    }

    return null;
  }
}