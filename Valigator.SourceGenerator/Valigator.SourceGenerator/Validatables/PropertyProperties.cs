namespace Valigator.SourceGenerator.Validatables;

internal record PropertyProperties(
	string PropertyName,
	string PropertyType,
	EquatableArray<ValidatorAttributeProperties> ValidationAttributes
);
