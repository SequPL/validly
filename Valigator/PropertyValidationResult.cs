using System.Collections.Immutable;

namespace Valigator;

/// <summary>
/// Object holding the result of a property validation
/// </summary>
public class PropertyValidationResult : IInternalPropertyValidationResult
{
	private bool? _success;
	private string _propertyName;

	/// <summary>
	/// Name of the validated property
	/// </summary>
	public string PropertyName => _propertyName;

	/// <summary>
	/// Error messages generated during validation
	/// </summary>
	public IReadOnlyCollection<ValidationMessage> Messages { get; }

	/// <summary>
	/// True if validation of this property was successful
	/// </summary>
	public bool Success => _success ??= Messages.Count == 0;

	/// <param name="propertyName"></param>
	/// <param name="messages"></param>
	public PropertyValidationResult(string propertyName, IEnumerable<ValidationMessage> messages)
	{
		_propertyName = propertyName;
		Messages = messages.ToImmutableArray(); // TODO: Test AnyAbleEnumerator
	}

	/// <param name="propertyName"></param>
	/// <param name="messages"></param>
	public PropertyValidationResult(string propertyName, params IEnumerable<ValidationMessage>[] messages)
	{
		_propertyName = propertyName;
		Messages = messages.SelectMany(x => x).ToImmutableArray(); // TODO: Test AnyAbleEnumerator
	}

	PropertyValidationResult IInternalPropertyValidationResult.AsNestedPropertyValidationResult(
		string parentPropertyName
	)
	{
		_propertyName = $"{parentPropertyName}.{_propertyName}";
		return this;
	}
}
