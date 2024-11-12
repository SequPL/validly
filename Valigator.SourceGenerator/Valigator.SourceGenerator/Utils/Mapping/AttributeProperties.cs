namespace Valigator.SourceGenerator.Utils.Mapping;

/// <summary>
/// Properties of attribute
/// </summary>
/// <remarks>
/// Example: [Range(5, 10)] -> new ValidationAttributeProperties("Range", "5, 10")
/// </remarks>
internal record AttributeProperties
{
	/// <summary></summary>
	public required string QualifiedName { get; init; }

	/// <summary>
	/// Arguments passed to the attribute
	/// </summary>
	public required EquatableArray<string> Arguments { get; init; }
}
