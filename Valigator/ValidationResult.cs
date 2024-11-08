using System.Collections.Immutable;

namespace Valigator;

/// <summary>
/// Represents the result of a validation
/// </summary>
/// <param name="propertiesResult"></param>
public class ValidationResult(
	IEnumerable<ValidationMessage> globalMessages,
	params PropertyValidationResult[] propertiesResult
)
{
	private IReadOnlyCollection<ValidationMessage>? _globalMessages;
	private IReadOnlyCollection<PropertyValidationResult>? _propertiesResult;

	private bool? _valid;

	/// <summary>
	/// List of global validation messages
	/// </summary>
	public IReadOnlyCollection<ValidationMessage> Global =>
		_globalMessages ??= globalMessages is ImmutableArray<ValidationMessage> immutableArray
			? immutableArray
			: globalMessages.ToImmutableArray();

	/// <summary>
	/// List of properties validation results
	/// </summary>
	public IReadOnlyCollection<PropertyValidationResult> Properties =>
		_propertiesResult ??= propertiesResult.ToImmutableArray();

	/// <summary>
	/// True if validation was successful
	/// </summary>
	public bool Valid => _valid ??= propertiesResult.All(prop => !prop.Messages.Any());
}
