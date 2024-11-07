namespace Valigator;

/// <summary>
/// Attribute to mark a class as validatable
/// </summary>
/// <remarks>
/// It serves as a marker for a source generator to know that the class is validatable.
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class ValidatableAttribute : Attribute;
