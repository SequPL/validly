using System.Buffers;
using System.Collections.ObjectModel;
using Microsoft.Extensions.ObjectPool;
using Valigator.Utils;

namespace Valigator;

/// <summary>
/// Object holding the result of a property validation
/// </summary>
public class PropertyValidationResult : IInternalPropertyValidationResult, IDisposable, IResettable
{
	private static readonly FinalizableObjectPool<PropertyValidationResult> Pool =
		FinalizableObjectPool.Create<PropertyValidationResult>();

	private static readonly ArrayPool<ValidationMessage> ValidationMessagePool = ArrayPool<ValidationMessage>.Shared;

	private bool? _success;
	private string _propertyName = null!;
	private IReadOnlyList<ValidationMessage>? _messages;

	// private AnyAbleLazyEnumerator<ValidationMessage>? _messagesEnumerable;
	private IEnumerable<ValidationMessage>? _messagesEnumerable;
	private ValidationMessage[] _messagesArray = null!;
	private int _messagesArrayItemCount;

	/// <summary>
	/// Name of the validated property
	/// </summary>
	public string PropertyName => _propertyName;

	/// <summary>
	/// Error messages generated during validation
	/// </summary>
	public IReadOnlyCollection<ValidationMessage> Messages => _messages ??= GetMessages();

	private IReadOnlyList<ValidationMessage> GetMessages()
	{
		if (_messagesEnumerable is not null)
		{
			// Copy messages from enumerable to array
			// !! Messages that do not fit in the array are discarded !!
			// _messagesEnumerable.Items.CopyTo(_messagesArray, Math.Min(_messagesArrayItemCount, _messagesArray.Length));
			// _messagesArrayItemCount += _messagesEnumerable.Items.Count;

			foreach (ValidationMessage validationMessage in _messagesEnumerable)
			{
				if (_messagesArrayItemCount < _messagesArray.Length)
				{
					_messagesArray[_messagesArrayItemCount++] = validationMessage;
				}
			}
		}

		return new ReadOnlyCollection<ValidationMessage>(_messagesArray.AsSpan(0, _messagesArrayItemCount).ToArray());
	}

	/// <summary>
	/// True if validation of this property was successful
	/// </summary>
	public bool Success => _success ??= _messagesArrayItemCount == 0 && !(_messagesEnumerable?.Any() ?? false);

	static PropertyValidationResult() { }

	/// <summary>
	/// Creates new instance of <see cref="PropertyValidationResult"/>
	/// </summary>
	/// <param name="propertyName"></param>
	/// <param name="enumerableMessages"></param>
	/// <returns></returns>
	public static PropertyValidationResult Create(
		string propertyName,
		IEnumerable<ValidationMessage>? enumerableMessages = null
	)
	{
		var result = Pool.Get();
		result.Reset(propertyName);

		if (enumerableMessages is not null)
		{
			result._messagesEnumerable = enumerableMessages; //new AnyAbleLazyEnumerator<ValidationMessage>(enumerableMessages);
		}

		return result;
	}

	private void Reset(string propertyName)
	{
		_propertyName = propertyName;
		_messagesArray = ValidationMessagePool.Rent(ValigatorConfig.PropertyMessagesPoolSize);
	}

	/// <summary>
	/// Add message to the validation result
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	public PropertyValidationResult AddValidationMessage(ValidationMessage? message)
	{
		if (message is not null && _messagesArrayItemCount < _messagesArray.Length)
		{
			_messagesArray[_messagesArrayItemCount++] = message;
		}

		return this;
	}

	PropertyValidationResult IInternalPropertyValidationResult.AsNestedPropertyValidationResult(
		string parentPropertyName
	)
	{
		_propertyName = $"{parentPropertyName}.{_propertyName}";
		return this;
	}

	/// <inheritdoc />
	bool IResettable.TryReset()
	{
		_propertyName = null!;
		_success = null;
		_messages = null;
		_messagesEnumerable = null;
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
		// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
		if (_messagesArray is not null)
		{
			ValidationMessagePool.Return(_messagesArray);
		}

		// if (disposing)
		// {
		// 	_messagesEnumerable?.Dispose();
		// }

		_messagesArray = null!;
		_messagesEnumerable = null;

		// return true;
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

	~PropertyValidationResult()
	{
		if (!Dispose(false))
		{
			// Reregister for finalization if not disposed (returned to the pool)
			GC.ReRegisterForFinalize(this);
		}
	}
}
