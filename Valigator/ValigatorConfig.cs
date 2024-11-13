namespace Valigator;

/// <summary>
/// Configuration
/// </summary>
public static class ValigatorConfig
{
	/// <summary>
	/// Size of the ArrayPool for messages in the <see cref="PropertyValidationResult"/>
	/// </summary>
	public static int PropertyMessagesPoolSize { get; set; } = 16;

	/// <summary>
	/// Max number of items of <see cref="Valigator.Utils.FinalizableObjectPool{TItem}"/>
	/// </summary>
	public static int ObjectPoolSize { get; set; } = Environment.ProcessorCount * 2;
}
