using Valigator.SourceGenerator.Utils;

namespace Valigator.SourceGenerator.Validatables.ObjectDtos;

/// <summary>
///
/// </summary>
/// <param name="PropertyName"></param>
/// <param name="PropertyType"></param>
/// <param name="IsValidatable">Property is of type that is Validatable</param>
/// <param name="ValidationAttributes"></param>
internal record PropertyProperties(
	string PropertyName,
	string PropertyType,
	bool IsValidatable,
	EquatableArray<ValidationAttributeProperties> ValidationAttributes
);
