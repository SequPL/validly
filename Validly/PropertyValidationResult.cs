using System.Buffers;
using System.Collections.ObjectModel;
using Microsoft.Extensions.ObjectPool;
using Validly.Utils;

namespace Validly;

/// <summary>
/// Object holding the result of a property validation
/// </summary>
public class PropertyValidationResult : IInternalPropertyValidationResult, IDisposable, IResettable
{
	private static readonly FinalizableObjectPool<PropertyValidationResult> Pool =
		FinalizableObjectPool.Create<PropertyValidationResult>();

	private static readonly ArrayPool<ValidationMessage> ValidationMessagePool = ArrayPool<ValidationMessage>.Shared;

	private string _propertyPath = null!;
	private string _propertyDisplayName = null!;
	private IReadOnlyList<ValidationMessage>? _messages;
	private bool _disposed;

	private ValidationMessage[] _messagesArray = null!;
	private int _messagesArrayItemCount;

	/// <summary>
	/// Path of the validated property from the root object
	/// </summary>
	public string PropertyPath => _propertyPath;

	/// <summary>
	/// DisplayName of the validated property
	/// </summary>
	public string PropertyDisplayName => _propertyDisplayName;

	/// <summary>
	/// Error messages generated during validation
	/// </summary>
	public IReadOnlyCollection<ValidationMessage> Messages => _messages ??= GetMessages();

	/// <summary>
	/// True if validation of this property was successful
	/// </summary>
	public bool IsSuccess => _messagesArrayItemCount == 0;

	static PropertyValidationResult() { }

	/// <summary>
	/// Creates new instance of <see cref="PropertyValidationResult"/>
	/// </summary>
	/// <param name="propertyName"></param>
	/// <returns></returns>
	public static PropertyValidationResult Create(string propertyName)
	{
		var result = Pool.Get();
		result.Reset(propertyName);

		return result;
	}

	/// <summary>
	/// Creates new instance of <see cref="PropertyValidationResult"/>
	/// </summary>
	/// <param name="propertyName"></param>
	/// <param name="propertyDisplayName"></param>
	/// <returns></returns>
	public static PropertyValidationResult Create(string propertyName, string propertyDisplayName)
	{
		var result = Pool.Get();
		result.Reset(propertyName, propertyDisplayName);

		return result;
	}

	private void Reset(string propertyPath)
	{
		_disposed = false;
		_propertyPath = propertyPath;
		_propertyDisplayName = propertyPath;
		_messagesArray = ValidationMessagePool.Rent(ValidlyOptions.PropertyMessagesPoolSize);
	}

	private void Reset(string propertyPath, string propertyDisplayName)
	{
		_disposed = false;
		_propertyPath = propertyPath;
		_propertyDisplayName = propertyDisplayName;
		_messagesArray = ValidationMessagePool.Rent(ValidlyOptions.PropertyMessagesPoolSize);
	}

	private IReadOnlyList<ValidationMessage> GetMessages()
	{
		return new ReadOnlyCollection<ValidationMessage>(_messagesArray.AsSpan(0, _messagesArrayItemCount).ToArray());
	}

	/// <summary>
	/// Add message to the validation result
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	public void AddValidationMessage(ValidationMessage? message)
	{
		if (message is not null && _messagesArrayItemCount < _messagesArray.Length)
		{
			_messagesArray[_messagesArrayItemCount++] = message;
		}
	}

	/// <summary>
	/// Add message to the validation result
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	public void AddValidationMessage(Validation? message)
	{
		if (message?.IsSuccess == false && _messagesArrayItemCount < _messagesArray.Length)
		{
			_messagesArray[_messagesArrayItemCount++] = message.Message;
		}
	}

	/// <summary>
	/// Add messages to the validation result
	/// </summary>
	/// <param name="messages"></param>
	/// <returns></returns>
	public void AddValidationMessages(IEnumerable<ValidationMessage> messages)
	{
		foreach (ValidationMessage message in messages)
		{
			if (_messagesArrayItemCount >= _messagesArray.Length)
			{
				break;
			}

			_messagesArray[_messagesArrayItemCount++] = message;
		}
	}

	/// <summary>
	/// Add messages to the validation result
	/// </summary>
	/// <param name="messages"></param>
	/// <returns></returns>
	public async Task AddValidationMessages(IAsyncEnumerable<ValidationMessage> messages)
	{
		await foreach (ValidationMessage message in messages)
		{
			if (_messagesArrayItemCount >= _messagesArray.Length)
			{
				break;
			}

			_messagesArray[_messagesArrayItemCount++] = message;
		}
	}

	PropertyValidationResult IInternalPropertyValidationResult.AsNestedPropertyValidationResult(
		string parentPropertyName
	)
	{
		_propertyPath = $"{parentPropertyName}.{_propertyPath}";
		return this;
	}

	/// <inheritdoc />
	bool IResettable.TryReset()
	{
		_propertyPath = null!;
		_propertyDisplayName = null!;
		_messages = null;
		_messagesArrayItemCount = 0;
		return true;
	}

	/// <summary>
	/// Tries to return the object to the pool; otherwise it will dispose it so it can be garbage collected.
	/// </summary>
	/// <param name="disposing"></param>
	/// <returns>Returns true when object is disposed (and not returned to the pool).</returns>
	private bool Dispose(bool disposing)
	{
		if (_disposed)
		{
			return true;
		}

		// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
		if (_messagesArray is not null)
		{
			ValidationMessagePool.Return(_messagesArray);
		}

		_messagesArray = null!;
		_disposed = true;

		return !Pool.Return(this);
	}

	/// <inheritdoc />
	public void Dispose()
	{
		if (Dispose(true))
		{
			// If disposed, suppress finalization
			GC.SuppressFinalize(this);
		}
	}

	/// <summary>
	/// Dispose the object using finalize for case developer forgets to dispose it.
	/// </summary>
	~PropertyValidationResult()
	{
		if (!Dispose(false))
		{
			// Reregister for finalization if not disposed (returned to the pool)
			GC.ReRegisterForFinalize(this);
		}
	}
}
