using System.Collections.ObjectModel;

namespace Valigator;

/// <summary>
/// Represents the result of a validation
/// </summary>
public class ValidationResult
{
	protected internal IList<ValidationMessage> GlobalMessages;
	protected internal IList<PropertyValidationResult> PropertiesResult;

	private bool? _success;

	/// <summary>
	/// List of global validation messages
	/// </summary>
	public IReadOnlyCollection<ValidationMessage> Global => new ReadOnlyCollection<ValidationMessage>(GlobalMessages);

	/// <summary>
	/// List of properties validation results
	/// </summary>
	public IReadOnlyCollection<PropertyValidationResult> Properties =>
		new ReadOnlyCollection<PropertyValidationResult>(PropertiesResult);

	/// <summary>
	/// True if validation was successful
	/// </summary>
	public bool Success => _success ??= !Global.Any() && PropertiesResult.All(prop => prop.Success);

	/// <summary>
	/// Represents the result of a validation
	/// </summary>
	/// <param name="globalMessages"></param>
	/// <param name="propertiesResult"></param>
	public ValidationResult(IList<ValidationMessage> globalMessages, params PropertyValidationResult[] propertiesResult)
	{
		GlobalMessages = globalMessages;
		PropertiesResult = propertiesResult;
	}

	internal ValidationResult(IList<ValidationMessage> globalMessages, IList<PropertyValidationResult> propertiesResult)
	{
		GlobalMessages = globalMessages;
		PropertiesResult = propertiesResult;
	}
}
