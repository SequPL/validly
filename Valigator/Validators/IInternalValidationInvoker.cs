namespace Valigator.Validators;

/// <summary>
/// Interface for internal use to invoke validation of nested objects
/// </summary>
public interface IInternalValidationInvoker
{
	/// <summary>
	/// Validates the object
	/// </summary>
	/// <param name="validationContext"></param>
	/// <param name="serviceProvider"></param>
	/// <returns></returns>
	ValueTask<ValidationResult> Validate(ValidationContext validationContext, IServiceProvider? serviceProvider);
}
