using Valigator.SourceGenerator.Utils;
using Valigator.SourceGenerator.Utils.Mapping;

namespace Valigator.SourceGenerator.Validatables.Dtos;

internal record ObjectProperties
{
	/// <summary>
	/// Usings from the validator's file
	/// </summary>
	public required EquatableArray<string> Usings { get; init; }

	// /// <summary>
	// /// Item per constructor. Item is parameters syntax (eg "int min, int max")
	// /// </summary>
	// public EquatableArray<string> Ctors { get; init; } = EquatableArray<string>.Empty;

	public required string ClassOrRecordKeyword { get; init; }

	/// <summary>
	/// Namespace of the validator
	/// </summary>
	public required string Namespace { get; init; }

	/// <summary>
	/// Name of the validator without the "Validator" suffix
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Object inherits
	/// </summary>
	public required bool InheritsValidatableObject { get; init; }

	/// <summary>
	/// List of properties
	/// </summary>
	public required EquatableArray<ValidatablePropertyProperties> Properties { get; init; }

	/// <summary>
	/// List of methods
	/// </summary>
	public required EquatableArray<MethodProperties> Methods { get; init; }

	/// <summary>
	/// BeforeValidate() method
	/// </summary>
	public required MethodProperties? BeforeValidateMethod { get; init; }

	/// <summary>
	/// AfterValidate() method
	/// </summary>
	public required MethodProperties? AfterValidateMethod { get; init; }

	// /// <summary>
	// /// Object has BeforeValidate method
	// /// </summary>
	// public required bool HasBeforeValidate { get; init; }
	//
	// /// <summary>
	// /// Return type of BeforeValidate() method
	// /// </summary>
	// public required BeforeValidateReturnType BeforeValidateReturnType { get; init; }
	//
	// /// <summary>
	// /// Object has AfterValidate method
	// /// </summary>
	// public required bool HasAfterValidate { get; init; }
	//
	// /// <summary>
	// /// Return type of AfterValidate() method
	// /// </summary>
	// public required AfterValidateReturnType AfterValidateReturnType { get; init; }

	// /// <summary>
	// /// Inherited object requires context
	// /// </summary>
	// public bool InheritedValidatableObjectRequiresContext { get; init; }
}
