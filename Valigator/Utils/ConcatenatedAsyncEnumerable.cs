namespace Valigator.Utils;

/// <summary>
/// Memory-optimized concatenator of enumerables
/// </summary>
public static class ConcatenatedAsyncEnumerable
{
	/// <summary>
	/// Concatenate multiple enumerables in a memory-optimized way
	/// </summary>
	public static IAsyncEnumerable<TItem> From<TItem>(IAsyncEnumerable<TItem> collection1)
	{
		return collection1;
	}

	/// <summary>
	/// Concatenate multiple enumerables in a memory-optimized way
	/// </summary>
	public static async IAsyncEnumerable<TItem> From<TItem>(
		IAsyncEnumerable<TItem> collection1,
		IAsyncEnumerable<TItem> collection2
	)
	{
		await foreach (TItem item in collection1)
		{
			yield return item;
		}

		await foreach (TItem item in collection2)
		{
			yield return item;
		}
	}

	/// <summary>
	/// Concatenate multiple enumerables in a memory-optimized way
	/// </summary>
	public static async IAsyncEnumerable<TItem> From<TItem>(
		IAsyncEnumerable<TItem> collection1,
		IAsyncEnumerable<TItem> collection2,
		IAsyncEnumerable<TItem> collection3
	)
	{
		await foreach (TItem item in collection1)
		{
			yield return item;
		}

		await foreach (TItem item in collection2)
		{
			yield return item;
		}

		await foreach (TItem item in collection3)
		{
			yield return item;
		}
	}

	/// <summary>
	/// Concatenate multiple enumerables in a memory-optimized way
	/// </summary>
	public static async IAsyncEnumerable<TItem> From<TItem>(
		IAsyncEnumerable<TItem> collection1,
		IAsyncEnumerable<TItem> collection2,
		IAsyncEnumerable<TItem> collection3,
		IAsyncEnumerable<TItem> collection4
	)
	{
		await foreach (TItem item in collection1)
		{
			yield return item;
		}

		await foreach (TItem item in collection2)
		{
			yield return item;
		}

		await foreach (TItem item in collection3)
		{
			yield return item;
		}

		await foreach (TItem item in collection4)
		{
			yield return item;
		}
	}

	/// <summary>
	/// Concatenate multiple enumerables in a memory-optimized way
	/// </summary>
	public static async IAsyncEnumerable<TItem> From<TItem>(
		IAsyncEnumerable<TItem> collection1,
		IAsyncEnumerable<TItem> collection2,
		IAsyncEnumerable<TItem> collection3,
		IAsyncEnumerable<TItem> collection4,
		IAsyncEnumerable<TItem> collection5
	)
	{
		await foreach (TItem item in collection1)
		{
			yield return item;
		}

		await foreach (TItem item in collection2)
		{
			yield return item;
		}

		await foreach (TItem item in collection3)
		{
			yield return item;
		}

		await foreach (TItem item in collection4)
		{
			yield return item;
		}

		await foreach (TItem item in collection5)
		{
			yield return item;
		}
	}

	/// <summary>
	/// Concatenate multiple enumerables in a memory-optimized way.
	/// This is fallback for big number of parameters with "params".
	/// This is not memory-optimized anymore cuz it requires construction of the array holding the enumerables;
	/// that's worse than doing collection1.Concat(collection2).Concat(collection3) etc.
	/// </summary>
	public static async IAsyncEnumerable<TItem> From<TItem>(params IAsyncEnumerable<TItem>[] collections)
	{
		foreach (var collection in collections)
		{
			await foreach (TItem item in collection)
			{
				yield return item;
			}
		}
	}
}