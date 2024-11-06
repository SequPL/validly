using System.Collections;
using System.Collections.Immutable;

namespace Valigator.SourceGenerator;

/// <summary>
/// An immutable, equatable array. This is equivalent to <see cref="ImmutableArray{T}"/> but with value equality support.
/// </summary>
/// <remarks>
/// From: https://github.com/andrewlock/StronglyTypedId/blob/master/src/StronglyTypedIds/EquatableArray.cs
/// </remarks>
/// <typeparam name="TItem">The type of values in the array.</typeparam>
internal readonly struct EquatableArray<TItem> : IEquatable<EquatableArray<TItem>>, IEnumerable<TItem>
	where TItem : IEquatable<TItem>
{
	public static readonly EquatableArray<TItem> Empty = new(Array.Empty<TItem>());

	/// <summary>
	/// The underlying <typeparamref name="TItem"/> array.
	/// </summary>
	private readonly TItem[]? _array;

	/// <summary>
	/// Creates a new <see cref="EquatableArray{T}"/> instance.
	/// </summary>
	/// <param name="array">The input <see cref="ImmutableArray"/> to wrap.</param>
	public EquatableArray(TItem[] array)
	{
		_array = array;
	}

	/// <sinheritdoc/>
	public bool Equals(EquatableArray<TItem> array)
	{
		return AsSpan().SequenceEqual(array.AsSpan());
	}

	/// <sinheritdoc/>
	public override bool Equals(object? obj)
	{
		return obj is EquatableArray<TItem> array && Equals(array);
	}

	/// <sinheritdoc/>
	public override int GetHashCode()
	{
		if (_array is not TItem[] array)
		{
			return 0;
		}

		HashCode hashCode = default;

		foreach (TItem item in array)
		{
			hashCode.Add(item);
		}

		return hashCode.ToHashCode();
	}

	/// <summary>
	/// Returns a <see cref="ReadOnlySpan{T}"/> wrapping the current items.
	/// </summary>
	/// <returns>A <see cref="ReadOnlySpan{T}"/> wrapping the current items.</returns>
	public ReadOnlySpan<TItem> AsSpan()
	{
		return _array.AsSpan();
	}

	/// <summary>
	/// Gets the underlying array if there is one
	/// </summary>
	public TItem[]? GetArray() => _array;

	/// <sinheritdoc/>
	IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
	{
		return ((IEnumerable<TItem>)(_array ?? Array.Empty<TItem>())).GetEnumerator();
	}

	/// <sinheritdoc/>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<TItem>)(_array ?? Array.Empty<TItem>())).GetEnumerator();
	}

	public int Count => _array?.Length ?? 0;

	/// <summary>
	/// Checks whether two <see cref="EquatableArray{T}"/> values are the same.
	/// </summary>
	/// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
	/// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
	/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
	public static bool operator ==(EquatableArray<TItem> left, EquatableArray<TItem> right)
	{
		return left.Equals(right);
	}

	/// <summary>
	/// Checks whether two <see cref="EquatableArray{T}"/> values are not the same.
	/// </summary>
	/// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
	/// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
	/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
	public static bool operator !=(EquatableArray<TItem> left, EquatableArray<TItem> right)
	{
		return !left.Equals(right);
	}
}
