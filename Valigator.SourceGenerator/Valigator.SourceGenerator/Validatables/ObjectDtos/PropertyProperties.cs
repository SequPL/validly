using Valigator.SourceGenerator.Utils;

namespace Valigator.SourceGenerator.Validatables.ObjectDtos;

internal record PropertyProperties(
	string PropertyName,
	string PropertyType,
	EquatableArray<ValidationAttributeProperties> ValidationAttributes
);
