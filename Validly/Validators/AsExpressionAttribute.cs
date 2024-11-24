namespace Validly.Validators;

/// <summary>
/// Mark parameters of validators' constructors with this attribute to indicate that they should be resolved as an expression
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class AsExpressionAttribute : Attribute;
