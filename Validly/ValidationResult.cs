using System.Runtime.CompilerServices;
using Validly.Details;
using Validly.Utils;

namespace Validly;

/// <summary>
/// Represents the result of a validation
/// </summary>
public class ValidationResult : IDisposable, IInternalValidationResult
{
	private static readonly ValidationResult SuccessResult =
		new(
			// ReSharper disable once UseCollectionExpression
			Array.Empty<ValidationMessage>(),
			PropertyValidationResultCollection.Empty
		);

	/// <summary>
	/// Global validation messages; messages that are not tied to a specific property
	/// </summary>
	protected internal ValidationMessage[] GlobalMessages;

	/// <summary>
	/// Number of items in <see cref="GlobalMessages"/> array
	/// </summary>
	protected internal int GlobalMessagesCount;

	/// <summary>
	/// Properties validation results
	/// </summary>
	protected internal PropertyValidationResultCollection PropertiesResultCollection;

	/// <summary>
	/// When true, the object has been disposed and is inside the pool
	/// </summary>
	private bool _disposed;

	/// <summary>
	/// List of global validation messages
	/// </summary>
	public IReadOnlyList<ValidationMessage> Global =>
		new SpanCollection<ValidationMessage>(
			GlobalMessages,
			0,
			Math.Max(GlobalMessagesCount - 1, 0),
			GlobalMessagesCount
		);

	/// <summary>
	/// List of properties validation results
	/// </summary>
	public IReadOnlyList<PropertyValidationResult> Properties => PropertiesResultCollection;

	/// <summary>
	/// True if validation was successful
	/// </summary>
	public bool IsSuccess => GlobalMessagesCount == 0 && PropertiesResultCollection.IsSuccess;

	/// <summary>
	/// Represents the result of a validation
	/// </summary>
	/// <param name="globalMessages"></param>
	public ValidationResult(ValidationMessage[] globalMessages)
	{
		GlobalMessages = globalMessages;
		GlobalMessagesCount = globalMessages.Length;
		PropertiesResultCollection = PropertyValidationResultCollection.Empty;
	}

	/// <summary>
	/// Represents the result of a validation
	/// </summary>
	/// <param name="globalMessages"></param>
	public ValidationResult(ICollection<ValidationMessage> globalMessages)
		: this(globalMessages.ToArray()) { }

	/// <param name="globalMessages"></param>
	/// <param name="propertiesResult"></param>
	private ValidationResult(ValidationMessage[] globalMessages, PropertyValidationResultCollection propertiesResult)
	{
		GlobalMessages = globalMessages;
		GlobalMessagesCount = globalMessages.Length;
		PropertiesResultCollection = propertiesResult;
	}

	/// <summary>
	/// Constructor for pooling
	/// </summary>
	/// <remarks>
	/// This ctor is danger. Not all fields are initialized.
	/// </remarks>
#pragma warning disable CS8618, CS9264
	protected ValidationResult() { }
#pragma warning restore CS8618, CS9264

	/// <summary>
	/// Success result
	/// </summary>
	public static ValidationResult Success() => SuccessResult;

	/// <summary>
	/// Copies data to a structure similar to the validation problem details defined in RFC 9457
	/// </summary>
	/// <returns></returns>
	public ValidationResultDetails GetProblemDetails()
	{
		return new ValidationResultDetails
		{
			Errors = PropertiesResultCollection.SelectMany(prop => prop.Messages.Select(error =>
				new ValidationErrorDetail
				{
					Detail = error.Message,
					ResourceKey = error.ResourceKey,
					Args = error.Args,
					Pointer = $"#{prop.PropertyPath}",
					FieldName = prop.PropertyDisplayName
				})).ToArray()
		};
	}

	/// <inheritdoc />
	int IInternalValidationResult.GetPropertiesCount() => PropertiesResultCollection.Count;

	/// <inheritdoc />
	IExpandablePropertyValidationResult IInternalValidationResult.InitProperty(string name, string? displayName)
	{
		return PropertiesResultCollection.InitProperty(name, displayName ?? name);
	}

	/// <summary>
	/// Add a global message to the validation result
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	void IInternalValidationResult.Add(Validation? message)
	{
		if (message is null || message.IsSuccess)
		{
			return;
		}

		AddGlobalMessageToArray(message.Message);
	}

	/// <summary>
	/// Add a global message to the validation result
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	void IInternalValidationResult.Add(ValidationMessage? message)
	{
		if (message is null)
		{
			return;
		}

		AddGlobalMessageToArray(message);
	}

	/// <summary>
	/// Add a global messages to the validation result
	/// </summary>
	/// <param name="messages"></param>
	/// <returns></returns>
	void IInternalValidationResult.Add(IEnumerable<ValidationMessage> messages)
	{
		foreach (ValidationMessage? message in messages)
		{
			AddGlobalMessageToArray(message);
		}
	}

	/// <summary>
	/// Add a global messages to the validation result
	/// </summary>
	/// <param name="messages"></param>
	/// <returns></returns>
	async Task IInternalValidationResult.AddAsync(IAsyncEnumerable<ValidationMessage> messages)
	{
		await foreach (ValidationMessage message in messages)
		{
			AddGlobalMessageToArray(message);
		}
	}

	void IInternalValidationResult.Combine(ValidationResult result)
	{
		// POP all messages from the result and add them to the current result
		for (int index = 0; index < result.GlobalMessagesCount; index++)
		{
			ValidationMessage message = result.GlobalMessages[index];
			AddGlobalMessageToArray(message);
		}

		// Reset count
		result.GlobalMessagesCount = 0;

		// Concat properties
		PropertiesResultCollection.Concat(result.PropertiesResultCollection);
	}

	void IInternalValidationResult.CombineNested(ValidationResult result, string propertyName)
	{
		// POP all messages from the result and add them to the current result
		for (int index = 0; index < result.GlobalMessagesCount; index++)
		{
			ValidationMessage message = result.GlobalMessages[index];
			AddGlobalMessageToArray(message);
		}

		// Reset count
		result.GlobalMessagesCount = 0;

		// POP all properties from the result and add them to the current result
		for (int index = 0; index < result.PropertiesResultCollection.Count; index++)
		{
			PropertyValidationResult propertyResult = result.PropertiesResultCollection[index];
			((IInternalPropertyValidationResult)propertyResult).AsNestedPropertyValidationResult(propertyName);
			// result.PropertiesResultCollection[index] = null!;
			PropertiesResultCollection.Add(propertyResult);
		}

		// // Reset count
		// result.PropertiesResultCollection.Count = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AddGlobalMessageToArray(ValidationMessage message)
	{
		if (GlobalMessagesCount >= GlobalMessages.Length)
		{
			throw new IndexOutOfRangeException("Collection is full.");
		}

		GlobalMessages[GlobalMessagesCount++] = message;
	}

	/// <summary>
	/// Dispose
	/// </summary>
	/// <param name="disposing"></param>
	protected virtual bool Dispose(bool disposing)
	{
		if (_disposed)
		{
			return false;
		}

		if (disposing)
		{
			PropertiesResultCollection.Dispose();
			PropertiesResultCollection = null!;
			GlobalMessages = null!;
		}

		_disposed = true;

		return false;
	}

	/// <inheritdoc />
	[DangerAfterDispose]
	public void Dispose()
	{
		if (Dispose(true))
		{
			// If disposed, suppress finalization
			GC.SuppressFinalize(this);
		}
	}
}
