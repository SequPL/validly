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

	// /// <summary>
	// /// Handles multiple source types and returns a ValidationResult
	// /// </summary>
	// /// <param name="validationResult"></param>
	// /// <returns></returns>
	// public static Task<ValidationResult?> ToValidationResult(Task<ValidationResult?> validationResult) =>
	// 	validationResult;

	/// <summary>
	/// Handles multiple source types and returns a ValidationResult
	/// </summary>
	/// <param name="errors"></param>
	/// <returns></returns>
	public static async Task<ValidationResult?> ToValidationResult(IAsyncEnumerable<ValidationMessage> errors)
	{
		var errorList = new List<ValidationMessage>(1);

		await foreach (var error in errors)
		{
			errorList.Add(error);
		}

		if (errorList.Count == 0)
		{
			return null;
		}

		return new ValidationResult(errorList);
	}

	/// <summary>
	/// Combine multiple results into a single result
	/// </summary>
	/// <param name="results"></param>
	/// <returns></returns>
	public static ValidationResult Combine(params ValidationResult[] results)
	{
		return new ValidationResult(
			results.SelectMany(r => r.Global).ToImmutableArray(),
			results.SelectMany(r => r.Properties).ToArray()
		);
	}

	/// <summary>
	/// Replace result by another result or add messages to existing result
	/// </summary>
	/// <remarks>
	/// Set of overloads to cover multiple scenarios.
	/// </remarks>
	/// <param name="result"></param>
	/// <param name="replaceOrAddResult"></param>
	/// <returns></returns>
	public static ValidationResult ReplaceOrAddMessages(
		ExtendableValidationResult result,
		ValidationResult? replaceOrAddResult
	)
	{
		if (replaceOrAddResult is not null)
		{
			return replaceOrAddResult;
		}

		return result;
	}

	/// <summary>
	/// Replace result by another result or add messages to existing result
	/// </summary>
	/// <remarks>
	/// Set of overloads to cover multiple scenarios.
	/// </remarks>
	/// <param name="result"></param>
	/// <param name="replaceOrAddResult"></param>
	/// <returns></returns>
	public static ValidationResult ReplaceOrAddMessages(
		ExtendableValidationResult result,
		IEnumerable<ValidationMessage> replaceOrAddResult
	)
	{
		foreach (var message in replaceOrAddResult)
		{
			result.AddGlobalMessage(message);
		}

		return result;
	}

	/// <summary>
	/// Replace result by another result or add messages to existing result
	/// </summary>
	/// <remarks>
	/// Set of overloads to cover multiple scenarios.
	/// </remarks>
	/// <param name="result"></param>
	/// <param name="replaceOrAddResult"></param>
	/// <returns></returns>
	public static async Task<ValidationResult> ReplaceOrAddMessages(
		ExtendableValidationResult result,
		IAsyncEnumerable<ValidationMessage> replaceOrAddResult
	)
	{
		await foreach (var message in replaceOrAddResult)
		{
			result.AddGlobalMessage(message);
		}

		return result;
	}
}
