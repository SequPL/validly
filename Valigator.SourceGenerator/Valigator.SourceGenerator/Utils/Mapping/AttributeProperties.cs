namespace Valigator.SourceGenerator.Utils.Mapping;

/// <summary>
/// Properties of attribute
/// </summary>
/// <remarks>
/// Example: [Range(5, 10)] -> new ValidationAttributeProperties("Range", "5, 10")
/// </remarks>
internal record AttributeProperties
{
	public static readonly AttributeProperties Required =
		new()
		{
			QualifiedName = "Valigator.Extensions.Validators.Common.RequiredAttribute",
			Arguments = EquatableArray<string>.Empty,
		};

	public static readonly AttributeProperties InEnum =
		new()
		{
			QualifiedName = "Valigator.Extensions.Validators.Enums.InEnumAttribute",
			Arguments = EquatableArray<string>.Empty,
		};

	/// <summary></summary>
	public required string QualifiedName { get; init; }

	/// <summary>
	/// Arguments passed to the attribute
	/// </summary>
	public required EquatableArray<string> Arguments { get; init; }
}
