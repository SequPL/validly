using Microsoft.CodeAnalysis;

namespace Valigator.SourceGenerator.Utils.Mapping;

internal record ValidatablePropertyProperties
{
	public required string PropertyName { get; init; }

	public required string PropertyType { get; init; }

	/// <summary>
	/// Kind of the type
	/// </summary>
	/// <remarks>
	/// May be class, struct, enum, interface, delegate, record,...
	/// </remarks>
	public required TypeKind PropertyTypeKind { get; init; }

	public required bool Nullable { get; init; }

	public required bool PropertyIsOfValidatableType { get; init; }

	public required EquatableArray<AttributeProperties> ValidationAttributes { get; init; }
}
