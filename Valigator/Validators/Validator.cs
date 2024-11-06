namespace Valigator.Validators;

/// <summary>
/// Base class for validators
/// </summary>
public abstract class Validator
{
	/// <summary>
	/// Validates the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public abstract IEnumerable<ValidationMessage> IsValid(object? value);
}
