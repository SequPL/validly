namespace Valigator;

/// <summary>
/// Validation context with information about the object being validated.
/// </summary>
public class ValidationContext
{
	private readonly object _rootObject;
	private object _object;
	private string _propertyName = string.Empty;

	/// <summary>
	/// Root object that is being validated.
	/// </summary>
	public object RootObject => _rootObject;

	/// <summary>
	/// Current object that is being validated. Is child of the root object.
	/// </summary>
	public object Object => _object;

	/// <summary>
	/// Name of current property that is being validated.
	/// </summary>
	public string PropertyName => _propertyName;

	/// <summary></summary>
	/// <param name="rootObject">Root validated object</param>
	public ValidationContext(object rootObject)
	{
		_rootObject = rootObject;
		_object = rootObject;
	}

	/// <summary>
	/// Change the current object that is being validated.
	/// </summary>
	/// <param name="obj"></param>
	public void SetObject(object obj)
	{
		_object = obj;
	}

	/// <summary>
	/// Change the current property that is being validated.
	/// </summary>
	/// <param name="propertyName"></param>
	public void SetProperty(string propertyName)
	{
		_propertyName = propertyName;
	}
}
