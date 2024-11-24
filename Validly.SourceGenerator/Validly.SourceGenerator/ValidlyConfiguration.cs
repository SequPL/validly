namespace Validly.SourceGenerator;

public record ValidlyConfiguration
{
	/// <summary>
	/// When enabled, the Required validator is automatically added to all properties that are not nullable
	/// </summary>
	public required bool AutoRequired { get; init; }

	/// <summary>
	/// When enabled, the InEnum validator is automatically added to all properties that are of enum type
	/// </summary>
	public required bool AutoInEnum { get; init; }

	// TODO: Implement this option
	/// <summary>
	/// When enabled, validation will exit early on the first validation failure
	/// </summary>
	public required bool ExitEarly { get; init; }
}
