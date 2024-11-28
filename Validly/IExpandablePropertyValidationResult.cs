namespace Validly;

/// <summary>
/// Interface allowing modification of the validation result
/// </summary>
public interface IExpandablePropertyValidationResult
{
	/// <summary>
	/// True if validation of this property was successful
	/// </summary>
	public bool IsSuccess { get; }

	/// <summary>
	/// Add message to the validation result
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	void Add(ValidationMessage? message);

	/// <summary>
	/// Add message to the validation result
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	void Add(Validation? message);

	/// <summary>
	/// Add messages to the validation result
	/// </summary>
	/// <param name="messages"></param>
	/// <returns></returns>
	void Add(IEnumerable<ValidationMessage> messages);

	/// <summary>
	/// Add messages to the validation result
	/// </summary>
	/// <param name="messages"></param>
	/// <returns></returns>
	Task AddAsync(IAsyncEnumerable<ValidationMessage> messages);
}
