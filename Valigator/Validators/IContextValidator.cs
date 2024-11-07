namespace Valigator.Validators;

/// <summary>
/// Interface for validators requiring ValidationContext (provides IServiceProvider for dependency injection
/// </summary>
public interface IContextValidator
{
	/// <summary>
	/// Validates the value
	/// </summary>
	/// <param name="value"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	IEnumerable<ValidationMessage> IsValid(object? value, ValidationContext context);
}