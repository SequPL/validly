namespace Valigator.SourceGenerator.Validatables;

internal class ObjectProperties
{
	/// <summary>
	/// Usings from the validator's file
	/// </summary>
	public EquatableArray<string> Usings { get; init; } = EquatableArray<string>.Empty;

	public EquatableArray<PropertyProperties> Properties { get; init; } = EquatableArray<PropertyProperties>.Empty;

	// /// <summary>
	// /// Item per constructor. Item is parameters syntax (eg "int min, int max")
	// /// </summary>
	// public EquatableArray<string> Ctors { get; init; } = EquatableArray<string>.Empty;

	public string ClassOrRecordKeyword { get; init; } = string.Empty;

	/// <summary>
	/// Namespace of the validator
	/// </summary>
	public string Namespace { get; init; } = string.Empty;

	/// <summary>
	/// Name of the validator without the "Validator" suffix
	/// </summary>
	public string Name { get; init; } = string.Empty;
}
