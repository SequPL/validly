// namespace Valigator.Utils;
//
// /// <summary>
// /// Memory-optimized concatenator of enumerables
// /// </summary>
// public static class ConcatenatedEnumerable
// {
// 	/// <summary>
// 	/// Concatenate multiple enumerables in a memory-optimized way
// 	/// </summary>
// 	public static IEnumerable<TItem> From<TItem>(IEnumerable<TItem> collection1)
// 	{
// 		return collection1;
// 	}
//
// 	/// <summary>
// 	/// Concatenate multiple enumerables in a memory-optimized way
// 	/// </summary>
// 	public static IEnumerable<TItem> From<TItem>(IEnumerable<TItem> collection1, IEnumerable<TItem> collection2)
// 	{
// 		foreach (TItem item in collection1)
// 		{
// 			yield return item;
// 		}
//
// 		foreach (TItem item in collection2)
// 		{
// 			yield return item;
// 		}
// 	}
//
// 	/// <summary>
// 	/// Concatenate multiple enumerables in a memory-optimized way
// 	/// </summary>
// 	public static IEnumerable<TItem> From<TItem>(
// 		IEnumerable<TItem> collection1,
// 		IEnumerable<TItem> collection2,
// 		IEnumerable<TItem> collection3
// 	)
// 	{
// 		foreach (TItem item in collection1)
// 		{
// 			yield return item;
// 		}
//
// 		foreach (TItem item in collection2)
// 		{
// 			yield return item;
// 		}
//
// 		foreach (TItem item in collection3)
// 		{
// 			yield return item;
// 		}
// 	}
//
// 	/// <summary>
// 	/// Concatenate multiple enumerables in a memory-optimized way
// 	/// </summary>
// 	public static IEnumerable<TItem> From<TItem>(
// 		IEnumerable<TItem> collection1,
// 		IEnumerable<TItem> collection2,
// 		IEnumerable<TItem> collection3,
// 		IEnumerable<TItem> collection4
// 	)
// 	{
// 		foreach (TItem item in collection1)
// 		{
// 			yield return item;
// 		}
//
// 		foreach (TItem item in collection2)
// 		{
// 			yield return item;
// 		}
//
// 		foreach (TItem item in collection3)
// 		{
// 			yield return item;
// 		}
//
// 		foreach (TItem item in collection4)
// 		{
// 			yield return item;
// 		}
// 	}
//
// 	/// <summary>
// 	/// Concatenate multiple enumerables in a memory-optimized way.
// 	/// This is fallback for big number of parameters with "params".
// 	/// This is not memory-optimized anymore cuz it requires construction of the array holding the enumerables;
// 	/// that's worse than doing collection1.Concat(collection2).Concat(collection3) etc.
// 	/// </summary>
// 	public static IEnumerable<TItem> From<TItem>(params IEnumerable<TItem>[] collections)
// 	{
// 		foreach (var collection in collections)
// 		{
// 			foreach (TItem item in collection)
// 			{
// 				yield return item;
// 			}
// 		}
// 	}
// }
