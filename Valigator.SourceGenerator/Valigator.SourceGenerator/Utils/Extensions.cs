using Microsoft.CodeAnalysis;

namespace Valigator.SourceGenerator.Utils;

public static class Extensions
{
	public static IncrementalValuesProvider<TSource> WhereNotNull<TSource>(
		this IncrementalValuesProvider<TSource?> source
	)
	{
		return source.Where(static item => item is not null)!;
	}

	public static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource?> source)
	{
		return source.Where(static item => item is not null)!;
	}
}
