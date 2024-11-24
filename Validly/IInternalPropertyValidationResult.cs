namespace Validly;

/// <summary>
/// Interface allowing some stuff intended for internal use
/// </summary>
public interface IInternalPropertyValidationResult
{
	/// <summary>
	/// This method will change the name of the property in the result.
	/// It will change PropertyName to "ParentPropertyName.PropertyName".
	/// </summary>
	/// <param name="parentPropertyName"></param>
	/// <returns></returns>
	PropertyValidationResult AsNestedPropertyValidationResult(string parentPropertyName);
}
