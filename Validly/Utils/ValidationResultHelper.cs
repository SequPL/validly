using System.Collections.Immutable;

namespace Validly.Utils;

/// <summary>
/// Set of helper methods for working with validation results
/// </summary>
public static class ValidationResultHelper
{
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
	public static async ValueTask<ValidationResult> ReplaceOrAddMessages(
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
