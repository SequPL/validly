namespace Valigator.SourceGenerator.Utils.Mapping;

internal record MethodProperties
{
	/// <summary>
	/// Name of the method
	/// </summary>
	public required string MethodName { get; init; }

	/// <summary>
	/// Name of the return type
	/// </summary>
	public required string ReturnType { get; init; }

	/// <summary>
	/// Name of the return type
	/// </summary>
	public required ReturnTypeType ReturnTypeType { get; init; }

	/// <summary>
	/// Name of the return type's generic argument
	/// </summary>
	public required string? ReturnTypeGenericArgument { get; init; }

	/// <summary>
	/// Dependencies of the method
	/// </summary>
	public required EquatableArray<string> Dependencies { get; init; }

	/// <summary>
	/// There are dependencies (other than ValidationContext) that need to be injected
	/// </summary>
	public required bool RequiresInjection { get; init; }

	// /// <summary>
	// /// Method requires async context
	// /// </summary>
	// public required bool IsAsync { get; init; }
	//
	// /// <summary>
	// /// Method is awaitable
	// /// </summary>
	// public required bool Awaitable { get; init; }
}
