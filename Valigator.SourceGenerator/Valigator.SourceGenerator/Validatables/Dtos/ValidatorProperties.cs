using Valigator.SourceGenerator.Utils.Mapping;

namespace Valigator.SourceGenerator.Validatables.Dtos;

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

	// /// <summary>
	// /// Namespace of the validator
	// /// </summary>
	// public required string Namespace { get; init; }
	//
	// /// <summary>
	// /// Name of the validator without the "Validator" suffix
	// /// </summary>
	// public required string Name { get; init; }

	// /// <summary>
	// /// Fully qualified name of the paired validation attribute
	// /// </summary>
	// public string PairedValidationAttributeFullyQualifiedName { get; init; } = string.Empty;

	// /// <summary>
	// /// True if the validator requires a ValidationContext
	// /// </summary>
	// [Obsolete]
	// public required bool RequiresContext { get; init; }

	// /// <summary>
	// /// Validator has dependencies
	// /// </summary>
	// public bool HasDependencies { get; init; }

	// public required EquatableArray<string> Dependencies { get; init; }
	//
	// /// <summary>
	// /// There are dependencies (other than ValidationContext) that need to be injected
	// /// </summary>
	// public required bool RequiresInjection { get; init; }
	//
	// /// <summary>
	// /// Validator is async
	// /// </summary>
	// public required bool IsAsync { get; init; }
}
