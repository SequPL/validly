namespace Validly.Details;

/// <summary>
/// Details of validation error
/// </summary>
public class ValidationErrorDetail
{
	/// <summary>
	/// Human-readable explanation
	/// </summary>
	public required string Detail { get; init; }

	/// <summary>
	/// Resource key for the <see cref="Detail"/> so it can be translated
	/// </summary>
	public required string ResourceKey { get; init; }

	/// <summary>
	/// Arguments used in the <see cref="Detail"/>, usable for <see cref="ResourceKey"/>
	/// </summary>
	public required object?[] Args { get; init; }

	/// <summary>
	/// Indicates a location of the data causing the validation error
	/// </summary>
	/// <remarks>
	/// JSON pointer - https://www.rfc-editor.org/rfc/rfc9457.html#RFC6901
	/// </remarks>
	public required string Pointer { get; init; }

	/// <summary>
	/// Name of the field
	/// </summary>
	public required string FieldName { get; init; }
}
