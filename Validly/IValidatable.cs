namespace Validly;

/// <summary>
/// Interface for validatable objects
/// </summary>
public interface IValidatable
{
	/// <summary>
	/// Validate the object
	/// </summary>
	/// <param name="serviceProvider"></param>
	/// <returns></returns>
	ValueTask<ValidationResult> Validate(IServiceProvider serviceProvider);
}
