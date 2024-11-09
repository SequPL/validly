using Valigator.SourceGenerator.Utils;

namespace Valigator.SourceGenerator.Validatables.ObjectDtos;

internal class ObjectProperties
{
	/// <summary>
	/// Usings from the validator's file
	/// </summary>
	public EquatableArray<string> Usings { get; init; } = EquatableArray<string>.Empty;

	/// <summary>
	/// List of properties
	/// </summary>
	public EquatableArray<PropertyProperties> Properties { get; init; } = EquatableArray<PropertyProperties>.Empty;

	/// <summary>
	/// List of methods
	/// </summary>
	public EquatableArray<MethodProperties> Methods { get; init; } = EquatableArray<MethodProperties>.Empty;

	// /// <summary>
	// /// Item per constructor. Item is parameters syntax (eg "int min, int max")
	// /// </summary>
	// public EquatableArray<string> Ctors { get; init; } = EquatableArray<string>.Empty;

	public string ClassOrRecordKeyword { get; init; } = string.Empty;

	/// <summary>
	/// Namespace of the validator
	/// </summary>
	public string Namespace { get; init; } = string.Empty;

	/// <summary>
	/// Name of the validator without the "Validator" suffix
	/// </summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>
	/// Object has BeforeValidate method
	/// </summary>
	public bool HasBeforeValidate { get; init; } = false;

	/// <summary>
	/// Return type of BeforeValidate() method
	/// </summary>
	public BeforeValidateReturnType BeforeValidateReturnType { get; init; } = BeforeValidateReturnType.Void;

	/// <summary>
	/// Object has AfterValidate method
	/// </summary>
	public bool HasAfterValidate { get; init; }

	/// <summary>
	/// Return type of AfterValidate() method
	/// </summary>
	public AfterValidateReturnType AfterValidateReturnType { get; init; } = AfterValidateReturnType.ValidationResult;

	/// <summary>
	/// Object inherits
	/// </summary>
	public bool InheritsValidatableObject { get; init; }

	// /// <summary>
	// /// Inherited object requires context
	// /// </summary>
	// public bool InheritedValidatableObjectRequiresContext { get; init; }
}
