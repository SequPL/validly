namespace Valigator;

/// <summary>
/// Interface for validatable objects
/// </summary>
public interface IValidatable
{
	/// <summary>
	/// Validate the object
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	ValueTask<ValidationResult> Validate(ValidationContext context);
}
