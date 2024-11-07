namespace Valigator.SourceGenerator.Validatables.ObjectDtos;

/// <summary>
/// Properties of used validation attribute on a property in validatable object
/// </summary>
/// <remarks>
/// Example: [Range(5, 10)] -> new ValidationAttributeProperties("GreaterThan", "5, 10", false)
/// </remarks>
/// <param name="FullyQualifiedName"></param>
/// <param name="Arguments">Arguments passed to validation attribute</param>
internal record ValidationAttributeProperties(string FullyQualifiedName, string Arguments);
