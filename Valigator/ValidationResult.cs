using System.Collections.ObjectModel;

namespace Valigator;

/// <summary>
/// Represents the result of a validation
/// </summary>
public class ValidationResult : IDisposable
{
	/// <summary>
	/// Success result
	/// </summary>
	public static readonly ValidationResult SuccessResult =
		new(Array.Empty<ValidationMessage>(), Array.Empty<PropertyValidationResult>());

	/// <summary>
	/// Global validation messages; messages that are not tied to a specific property
	/// </summary>
	protected internal IList<ValidationMessage> GlobalMessages;

	/// <summary>
	/// Properties validation results
	/// </summary>
	protected internal IList<PropertyValidationResult> PropertiesResult;

	/// <summary>
	/// Backing field for <see cref="IsSuccess"/>
	/// </summary>
	private bool? _isSuccess;

	/// <summary>
	/// When true, the object has been disposed and is inside the pool
	/// </summary>
	private bool _disposed;

	/// <summary>
	/// List of global validation messages
	/// </summary>
	public IReadOnlyCollection<ValidationMessage> Global => new ReadOnlyCollection<ValidationMessage>(GlobalMessages);

	/// <summary>
	/// List of properties validation results
	/// </summary>
	public IReadOnlyCollection<PropertyValidationResult> Properties =>
		new ReadOnlyCollection<PropertyValidationResult>(PropertiesResult);

	/// <summary>
	/// True if validation was successful
	/// </summary>
	public bool IsSuccess
	{
		get
		{
			// Mem optimized: _success ??= Global.Count == 0 && PropertiesResult.All(p => p.IsSuccess);
			if (_isSuccess == null)
			{
				if (GlobalMessages.Count != 0)
				{
					_isSuccess = false;
				}
				else
				{
					for (int index = 0; index < PropertiesResult.Count; index++)
					{
						if (!PropertiesResult[index].IsSuccess)
						{
							_isSuccess = false;
							break;
						}
					}
				}
			}

			return _isSuccess ??= true;
		}
	}

	/// <summary>
	/// Represents the result of a validation
	/// </summary>
	/// <param name="globalMessages"></param>
	public ValidationResult(IList<ValidationMessage> globalMessages)
	{
		GlobalMessages = globalMessages;
		PropertiesResult = Array.Empty<PropertyValidationResult>();
	}

	// /// <summary>
	// /// Represents the result of a validation
	// /// </summary>
	// protected ValidationResult() { }

	internal ValidationResult(IList<ValidationMessage> globalMessages, IList<PropertyValidationResult> propertiesResult)
	{
		GlobalMessages = globalMessages;
		PropertiesResult = propertiesResult;
	}

	/// <inheritdoc />
	[DangerAfterDispose]
	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;

		foreach (PropertyValidationResult propertyValidationResult in PropertiesResult)
		{
			propertyValidationResult.Dispose();
		}
	}
}
