namespace Validly.Details;

/// <summary>
/// Details of validation result
/// </summary>
public class ValidationResultDetails
{
	/// <summary>
	/// Details of validation errors
	/// </summary>
	public required IReadOnlyList<ValidationErrorDetail> Errors { get; init; }
}
