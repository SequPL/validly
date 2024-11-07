using Valigator.SourceGenerator.Utils;

namespace Valigator.SourceGenerator.Validatables.ValidatorDtos;

internal class ValidatorProperties
{
	/// <summary>
	/// Usings from the validator's file
	/// </summary>
	public EquatableArray<string> Usings { get; init; } = EquatableArray<string>.Empty;

	/// <summary>
	/// Namespace of the validator
	/// </summary>
	public string Namespace { get; init; } = string.Empty;

	/// <summary>
	/// Name of the validator without the "Validator" suffix
	/// </summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>
	/// Fully qualified name of the paired validation attribute
	/// </summary>
	public string PairedValidationAttributeFullyQualifiedName { get; init; } = string.Empty;

	/// <summary>
	/// True if the validator requires a ValidationContext
	/// </summary>
	public bool RequiresContext { get; init; }
}
