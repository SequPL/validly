namespace Valigator.Utils;

/// <summary>
/// Lazylly enumerates over a collection of items;
/// can tell you if there is any item without full enumeration or double enumerations.
/// </summary>
public class AnyAbleEnumerator<TItem> : IDisposable
{
	private readonly IEnumerator<TItem> _enumerator;
	private List<TItem> _items = new(1);

	// private bool _enumerated;

	/// <summary>
	/// Items in the collection
	/// </summary>
	public IReadOnlyList<TItem> Items
	{
		get
		{
			while (Enumerate()) { }
			return _items;
		}
	}

	/// <param name="enumerable"></param>
	public AnyAbleEnumerator(IEnumerable<TItem> enumerable)
	{
		_enumerator = enumerable.GetEnumerator();
	}

	/// <summary>
	/// Returns true if there is any item in the collection without full enumeration.
	/// </summary>
	/// <returns></returns>
	public bool Any()
	{
		return _items.Count != 0 || Enumerate();
	}

	private bool Enumerate()
	{
		// if (_enumerated)
		// {
		// 	return false;
		// }

		if (_enumerator.MoveNext())
		{
			_items.Add(_enumerator.Current);
			return true;
		}

		// _enumerated = true;

		return false;
	}

	/// <inheritdoc />
	public void Dispose()
	{
		_enumerator.Dispose();
	}
}
