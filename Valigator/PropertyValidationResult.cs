namespace Valigator;

/// <summary>
/// Object holding the result of a property validation
/// </summary>
public class PropertyValidationResult
{
	/// <summary>
	/// Name of the validated property
	/// </summary>
	public required string PropertyName { get; init; }

	/// <summary>
	/// Error messages generated during validation
	/// </summary>
	public IReadOnlyCollection<ValidationMessage> Messages { get; init; } = [];
}
