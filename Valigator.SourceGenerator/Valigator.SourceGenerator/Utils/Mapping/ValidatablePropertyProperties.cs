namespace Valigator.SourceGenerator.Utils.Mapping;

internal record ValidatablePropertyProperties
{
	public required string PropertyName { get; init; }

	public required string PropertyType { get; init; }

	public required bool PropertyIsOfValidatableType { get; init; }

	public required EquatableArray<AttributeProperties> ValidationAttributes { get; init; }
}
