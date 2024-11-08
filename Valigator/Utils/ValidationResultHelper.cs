using System.Collections.Immutable;

namespace Valigator.Utils;

/// <summary>
/// Set of helper methods for working with validation results
/// </summary>
public static class ValidationResultHelper
{
	/// <summary>
	/// Handles multiple source types and returns a ValidationResult
	/// </summary>
	/// <param name="validationResult"></param>
	/// <returns></returns>
	public static ValidationResult? ToValidationResult(ValidationResult? validationResult) => validationResult;

	/// <summary>
	/// Handles multiple source types and returns a ValidationResult
	/// </summary>
	/// <param name="errors"></param>
	/// <returns></returns>
	public static ValidationResult? ToValidationResult(IEnumerable<ValidationMessage> errors)
	{
		var errorArray = errors.ToImmutableArray();

		if (errorArray.Length == 0)
		{
			return null;
		}

		return new ValidationResult(errorArray);
	}
}
