namespace Valigator;

/// <summary>
/// Marks a method as dangerous to call after the object has been disposed
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class DangerAfterDisposeAttribute : Attribute;