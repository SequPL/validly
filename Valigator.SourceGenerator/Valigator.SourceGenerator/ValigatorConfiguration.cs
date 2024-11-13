namespace Valigator.SourceGenerator;

public record ValigatorConfiguration
{
	/// <summary>
	/// When enabled, the Required validator is automatically added to all properties that are not nullable
	/// </summary>
	public required bool AutoRequired { get; init; }
}
