using System.Collections.ObjectModel;

namespace Valigator;

/// <summary>
/// Represents the result of a validation
/// </summary>
public class ValidationResult : IDisposable
{
	private static readonly ValidationResult SuccessResult =
		new(
			// ReSharper disable once UseCollectionExpression
			Array.Empty<ValidationMessage>(),
			// ReSharper disable once UseCollectionExpression
			Array.Empty<PropertyValidationResult>()
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
	protected internal PropertyValidationResult[] PropertiesResult;

	/// <summary>
	/// Number of items in <see cref="PropertiesResult"/> array
	/// </summary>
	protected internal int PropertiesResultCount;

	// /// <summary>
	// /// Backing field for <see cref="IsSuccess"/>
	// /// </summary>
	// protected bool? _isSuccess;

	/// <summary>
	/// When true, the object has been disposed and is inside the pool
	/// </summary>
	private bool _disposed;

	/// <summary>
	/// List of global validation messages
	/// </summary>
	public IReadOnlyCollection<ValidationMessage> Global =>
		new ReadOnlyCollection<ValidationMessage>(GlobalMessages.AsSpan(0, GlobalMessagesCount).ToArray());

	/// <summary>
	/// List of properties validation results
	/// </summary>
	public IReadOnlyCollection<PropertyValidationResult> Properties =>
		new ReadOnlyCollection<PropertyValidationResult>(PropertiesResult.AsSpan(0, PropertiesResultCount).ToArray());

	/// <summary>
	/// True if validation was successful
	/// </summary>
	public bool IsSuccess
	{
		get
		{
			// // Mem optimized: _success ??= Global.Count == 0 && PropertiesResult.All(p => p.IsSuccess);
			// if (_isSuccess == null)
			// {
			if (GlobalMessagesCount != 0)
			{
				return false;
				// _isSuccess = false;
			}
			// else
			// {
			for (int index = 0; index < PropertiesResultCount; index++)
			{
				if (!PropertiesResult[index].IsSuccess)
				{
					return false;
					// _isSuccess = false;
					// break;
				}
			}
			// }
			// }
			//
			// return _isSuccess ??= true;
			return true;
		}
	}

	/// <summary>
	/// Represents the result of a validation
	/// </summary>
	/// <param name="globalMessages"></param>
	public ValidationResult(ValidationMessage[] globalMessages)
	{
		GlobalMessages = globalMessages;
		GlobalMessagesCount = globalMessages.Length;

		// ReSharper disable once UseCollectionExpression
		PropertiesResult = Array.Empty<PropertyValidationResult>();
	}

	/// <summary>
	/// Represents the result of a validation
	/// </summary>
	/// <param name="globalMessages"></param>
	public ValidationResult(ICollection<ValidationMessage> globalMessages)
		: this(globalMessages.ToArray()) { }

	/// <param name="globalMessages"></param>
	/// <param name="propertiesResult"></param>
	private ValidationResult(ValidationMessage[] globalMessages, PropertyValidationResult[] propertiesResult)
	{
		GlobalMessages = globalMessages;
		GlobalMessagesCount = globalMessages.Length;

		PropertiesResult = propertiesResult;
		PropertiesResultCount = propertiesResult.Length;
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
			// // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
			// if (PropertiesResult != null)
			// {
			for (int index = 0; index < PropertiesResultCount; index++)
			{
				PropertyValidationResult propertyValidationResult = PropertiesResult[index];
				propertyValidationResult.Dispose();
			}

			PropertiesResult = null!;
			// }

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
