using Valigator.SourceGenerator.Utils;
using Valigator.SourceGenerator.Utils.Mapping;

namespace Valigator.SourceGenerator.Dtos;

internal record ObjectProperties
{
	/// <summary>
	/// Usings from the validator's file
	/// </summary>
	public required EquatableArray<string> Usings { get; init; }

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
}
