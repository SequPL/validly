namespace Valigator;

/// <summary>
/// Represents the result of a validation. Variant with methods for adding global messages and properties results.
/// </summary>
public class ExtendableValidationResult : ValidationResult
{
	/// <summary>
	/// Represents the result of a validation. Variant with methods for adding global messages and properties results.
	/// </summary>
	public ExtendableValidationResult(int propertiesCount)
		: base(new List<ValidationMessage>(), new List<PropertyValidationResult>(propertiesCount))
	{
	}

	/// <summary>
	/// Add a global message to the validation result
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	public ExtendableValidationResult AddGlobalMessage(ValidationMessage message)
	{
		GlobalMessages.Add(message);
		return this;
	}

	/// <summary>
	/// Add a global messages to the validation result
	/// </summary>
	/// <param name="messages"></param>
	/// <returns></returns>
	public ExtendableValidationResult AddGlobalMessages(IEnumerable<ValidationMessage> messages)
	{
		foreach (ValidationMessage? message in messages)
		{
			GlobalMessages.Add(message);
		}

		return this;
	}

	/// <summary>
	/// Add a global messages to the validation result
	/// </summary>
	/// <param name="messages"></param>
	/// <returns></returns>
	public async ValueTask<ExtendableValidationResult> AddGlobalMessages(IAsyncEnumerable<ValidationMessage> messages)
	{
		await foreach (ValidationMessage message in messages)
		{
			GlobalMessages.Add(message);
		}

		return this;
	}

	/// <summary>
	/// Add a property result to the validation result
	/// </summary>
	public ExtendableValidationResult AddPropertyResult(PropertyValidationResult propertyResult)
	{
		PropertiesResult.Add(propertyResult);
		return this;
	}
}