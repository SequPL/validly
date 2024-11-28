using System.Buffers;
using System.Collections;
using Microsoft.Extensions.ObjectPool;
using Validly.Utils;

namespace Validly;

/// <summary>
/// Collection of <see cref="PropertyValidationResult"/>s for all the properties of the object being validated.
/// </summary>
/// <remarks>
/// This class is pooled and should be created using <see cref="Create"/> method.
/// </remarks>
public class PropertyValidationResultCollection : IReadOnlyList<PropertyValidationResult>, IDisposable, IResettable
{
	/// <summary>
	/// Empty, readonly, collection
	/// </summary>
	public static readonly PropertyValidationResultCollection Empty =
#pragma warning disable CS0618 // Type or member is obsolete
		new()
		{
			_disposed = true,
			// ReSharper disable once UseCollectionExpression
			_propertiesResult = Array.Empty<PropertyValidationResult>(),
			// ReSharper disable once UseCollectionExpression
			_messages = Array.Empty<ValidationMessage>(),
		};
#pragma warning restore CS0618 // Type or member is obsolete

	/// <summary>
	/// Pool for this class
	/// </summary>
	private static readonly FinalizableObjectPool<PropertyValidationResultCollection> Pool =
		FinalizableObjectPool.Create<PropertyValidationResultCollection>();

	/// <summary>
	/// Pool for <see cref="PropertyValidationResult"/>s
	/// </summary>
	private static readonly ArrayPool<PropertyValidationResult> PropertyValidationResultPool =
		ArrayPool<PropertyValidationResult>.Shared;

	/// <summary>
	/// Pool for <see cref="ValidationMessage"/>s
	/// </summary>
	private static readonly ArrayPool<ValidationMessage> ValidationMessagePool = ArrayPool<ValidationMessage>.Shared;

	private bool _disposed;

	/// <summary>
	/// Array of all properties results
	/// </summary>
	/// <remarks>
	/// It is pooled collection. Real count of the items is stored in <see cref="_count"/>.
	/// Objects inside the array are part of the pool; they are reused and not reallocated.
	/// </remarks>
	private PropertyValidationResult[] _propertiesResult = null!;

	/// <summary>
	/// Count of items in <see cref="_propertiesResult"/> pooled array
	/// </summary>
	private int _count;

	/// <summary>
	/// Buffer of messages for all the properties separately stored in <see cref="_propertiesResult"/>
	/// </summary>
	/// <remarks>
	/// Messages are reassigned and not reused; this buffer is pooled so the array itself do not allocate memory.
	/// </remarks>
	private ValidationMessage[] _messages = null!;

	/// <summary>
	/// Size of the message buffer for one property
	/// </summary>
	private int _messagesPerProperty;

	/// <inheritdoc />
	public int Count => _count;

	/// <summary>
	/// True if validation of all the properties was successful
	/// </summary>
	// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
	public bool IsSuccess
	{
		get
		{
			for (int propIndex = 0; propIndex < _count; propIndex++)
			{
				if (!_propertiesResult[propIndex].IsSuccess)
				{
					return false;
				}
			}

			return true;
		}
	}

	/// <summary>
	/// Ctor for pooled objects
	/// </summary>
	[Obsolete("Use Create method instead.")]
	public PropertyValidationResultCollection() { }

	/// <summary>
	/// Creates new instance of <see cref="PropertyValidationResultCollection"/>
	/// </summary>
	/// <returns></returns>
	public static PropertyValidationResultCollection Create(int propertiesCount, int messagesPerProperty)
	{
		var collection = Pool.Get();
		collection._disposed = false;
		collection._messagesPerProperty = messagesPerProperty;
		collection._messages = ValidationMessagePool.Rent(propertiesCount * messagesPerProperty);
		collection._propertiesResult = PropertyValidationResultPool.Rent(propertiesCount);
		collection._count = 0;

		return collection;
	}

	/// <inheritdoc />
	bool IResettable.TryReset()
	{
		_count = 0;
		return true;
	}

	internal PropertyValidationResult InitProperty(string name, string displayName)
	{
		var propertyResult = _propertiesResult[_count++] ??=
			// ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
			new PropertyValidationResult();

		int start = Math.Max(0, _count - 1) * _messagesPerProperty;
		propertyResult.ResetProperty(
			name,
			displayName,
			_messages, start, start + _messagesPerProperty, 0
		);

		return propertyResult;
	}

	/// <summary>
	/// Concat this collection with another one. Items from the other collection are added to the end of this collection.
	/// </summary>
	/// <param name="collection"></param>
	/// <returns></returns>
	public PropertyValidationResultCollection Concat(PropertyValidationResultCollection collection)
	{
		if (_disposed)
		{
			throw new InvalidOperationException();
		}

		for (int index = 0; index < collection._count && _count < _messages.Length; index++)
		{
			_propertiesResult[_count++] = collection._propertiesResult[index];
		}

		// Reset counts to avoid double disposing
		collection._count = 0;

		return this;
	}

	/// <summary>
	/// Add property validation result to the collection
	/// </summary>
	/// <param name="propertyValidationResult"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public void Add(PropertyValidationResult propertyValidationResult)
	{
		if (_count >= _propertiesResult.Length)
		{
			throw new InvalidOperationException(
				"Collection is full. You probably created result for non-existing property."
			);
		}

		var existingPropertyResult = FindProperty(propertyValidationResult.PropertyPath);

		if (existingPropertyResult is null)
		{
			_propertiesResult[_count++] = propertyValidationResult;
			return;
		}

		var expandable = existingPropertyResult.AsExpandable();

		foreach (var message in propertyValidationResult.Messages)
		{
			expandable.Add(message);
		}
	}

	/// <summary>
	/// Find property by its path
	/// </summary>
	/// <param name="propertyPath"></param>
	/// <returns></returns>
	public PropertyValidationResult? FindProperty(string propertyPath)
	{
		for (int i = 0; i < _count; i++)
		{
			if (_propertiesResult[i].PropertyPath == propertyPath)
			{
				return _propertiesResult[i];
			}
		}

		return null;
	}

	/// <inheritdoc />
	public IEnumerator<PropertyValidationResult> GetEnumerator()
	{
		for (int i = 0; i < _count; i++)
		{
			yield return _propertiesResult[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <inheritdoc />
	public PropertyValidationResult this[int index]
	{
		get
		{
			if (index < 0 || index > _count)
			{
				throw new IndexOutOfRangeException();
			}

			return _propertiesResult[index];
		}
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
		if (_messages is not null)
		{
			ValidationMessagePool.Return(_messages, clearArray: true);
		}

		// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
		if (_propertiesResult is not null)
		{
			PropertyValidationResultPool.Return(_propertiesResult, clearArray: false);
		}

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
	~PropertyValidationResultCollection()
	{
		if (!Dispose(false))
		{
			// Reregister for finalization if not disposed (returned to the pool)
			GC.ReRegisterForFinalize(this);
		}
	}
}
