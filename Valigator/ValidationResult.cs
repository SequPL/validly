using System.Collections.Immutable;

namespace Valigator;

/// <summary>
/// Represents the result of a validation
/// </summary>
/// <param name="propertiesResult"></param>
public class ValidationResult(
	// IList<ValidationMessage> modelMessages, // TODO: Add something like this? Maybe a list of messages for the model as a whole?
	params PropertyValidationResult[] propertiesResult
)
{
	private bool? _valid;

	// public IReadOnlyCollection<ValidationMessage> Model => modelMessages.ToImmutableArray();

	/// <summary>
	/// List of properties validation results
	/// </summary>
	public IReadOnlyCollection<PropertyValidationResult> Properties => propertiesResult.ToImmutableArray();

	/// <summary>
	/// True if validation was successful
	/// </summary>
	public bool Valid => _valid ??= !propertiesResult.Any();
}
