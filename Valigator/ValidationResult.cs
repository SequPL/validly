using System.Collections.ObjectModel;

namespace Valigator;

/// <summary>
/// Represents the result of a validation
/// </summary>
public class ValidationResult : IDisposable
{
	/// <summary>
	/// Global validation messages; messages that are not tied to a specific property
	/// </summary>
	protected internal IList<ValidationMessage> GlobalMessages;

	/// <summary>
	/// Properties validation results
	/// </summary>
	protected internal IList<PropertyValidationResult> PropertiesResult;

	private bool? _success;

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
	public bool Success
	{
		get
		{
			// Mem optimized: _success ??= Global.Count == 0 && PropertiesResult.All(p => p.Success);
			if (_success == null)
			{
				if (GlobalMessages.Count != 0)
				{
					_success = false;
				}
				else
				{
					for (int index = 0; index < PropertiesResult.Count; index++)
					{
						if (!PropertiesResult[index].Success)
						{
							_success = false;
							break;
						}
					}
				}
			}

			return _success ??= true;
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
	public void Dispose()
	{
		foreach (PropertyValidationResult propertyValidationResult in PropertiesResult ?? [])
		{
			propertyValidationResult.Dispose();
		}
	}
}
