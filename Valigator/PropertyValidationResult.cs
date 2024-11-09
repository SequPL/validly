using System.Diagnostics.CodeAnalysis;

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
	public required string PropertyName
	{
		get => _propertyName;
		[MemberNotNull(nameof(_propertyName))]
		init => _propertyName = value;
	}

	/// <summary>
	/// Error messages generated during validation
	/// </summary>
	public IReadOnlyCollection<ValidationMessage> Messages { get; init; } = [];

	/// <summary>
	/// True if validation of this property was successful
	/// </summary>
	public bool Success => _success ??= Messages.Count == 0;

	PropertyValidationResult IInternalPropertyValidationResult.AsNestedPropertyValidationResult(
		string parentPropertyName
	)
	{
		_propertyName = $"{parentPropertyName}.{_propertyName}";
		return this;
	}
}
