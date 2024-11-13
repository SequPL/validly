using Valigator.SourceGenerator.Utils.Mapping;

namespace Valigator.SourceGenerator.Dtos;

internal record ValidatorProperties
{
	/// <summary>
	/// Qualified name of the Validator
	/// </summary>
	public required string QualifiedName { get; init; }

	/// <summary>
	/// IsValid method properties
	/// </summary>
	public required MethodProperties IsValidMethod { get; init; }
}
