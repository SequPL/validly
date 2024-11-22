namespace Valigator;

/// <summary>
/// Configuration
/// </summary>
public static class ValigatorOptions
{
	/// <summary>
	/// Size of the ArrayPool for <see cref="ValidationResult.Global"/> message collection
	/// </summary>
	public static int GlobalMessagesPoolSize { get; internal set; } = 16;

	/// <summary>
	/// Size of the ArrayPool for messages in the <see cref="PropertyValidationResult"/>
	/// </summary>
	public static int PropertyMessagesPoolSize { get; internal set; } = 16;

	/// <summary>
	/// Max number of items of <see cref="Valigator.Utils.FinalizableObjectPool{TItem}"/>
	/// </summary>
	public static int ObjectPoolSize { get; internal set; } = Environment.ProcessorCount * 2;

	/// <summary>
	/// Configure Valigator
	/// </summary>
	public static void Configure(Action<OptionsBuilder> configure)
	{
		configure(new OptionsBuilder());
	}
}

/// <summary>
/// Validator's Options builder
/// </summary>
public class OptionsBuilder
{
	/// <summary>
	/// Set size of the ArrayPool for <see cref="ValidationResult.Global"/> in <see cref="ValidationResult"/>
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	public OptionsBuilder WithGlobalMessagesPoolSize(int size)
	{
		ValigatorOptions.GlobalMessagesPoolSize = size;
		return this;
	}

	/// <summary>
	/// Set size of the ArrayPool for messages in the <see cref="PropertyValidationResult"/>
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	public OptionsBuilder WithPropertyMessagesPoolSize(int size)
	{
		ValigatorOptions.PropertyMessagesPoolSize = size;
		return this;
	}

	/// <summary>
	/// Set max number of items of <see cref="Valigator.Utils.FinalizableObjectPool{TItem}"/>
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	public OptionsBuilder WithObjectPoolSize(int size)
	{
		ValigatorOptions.ObjectPoolSize = size;
		return this;
	}
}
