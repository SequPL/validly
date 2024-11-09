using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Valigator.SourceGenerator.Utils.Symbols;

public class CustomSymbolVisitor
{
	public static ImmutableArray<INamedTypeSymbol> GetNamedTypes(
		IAssemblySymbol assemblySymbol,
		CancellationToken cancellationToken = default,
		Func<INamedTypeSymbol, bool>? predicate = null
	)
	{
		// Skip assemblies that don't reference Valigator
		if (
			!assemblySymbol.GlobalNamespace.ContainingModule?.ReferencedAssemblySymbols.Any(refAssembly =>
				refAssembly.Name == "Valigator"
			) ?? true
		)
		{
			return ImmutableArray<INamedTypeSymbol>.Empty;
		}

		return GetNamedTypes(assemblySymbol.GlobalNamespace, cancellationToken, predicate);
	}

	private static ImmutableArray<INamedTypeSymbol> GetNamedTypes(
		INamespaceSymbol namespaceSymbol,
		CancellationToken cancellationToken,
		Func<INamedTypeSymbol, bool>? predicate
	)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// Exclude System and Microsoft namespaces
		if (namespaceSymbol.Name.StartsWith("System") || namespaceSymbol.Name.StartsWith("Microsoft"))
		{
			return ImmutableArray<INamedTypeSymbol>.Empty;
		}

		IEnumerable<INamedTypeSymbol> namespaceTypes = namespaceSymbol.GetTypeMembers();

		namespaceTypes = predicate is not null
			? namespaceTypes.Where(t => FilterNonSystemNamedTypes(t) && predicate(t))
			: namespaceTypes.Where(FilterNonSystemNamedTypes);

		return namespaceTypes
			.Concat(
				namespaceSymbol.GetNamespaceMembers().SelectMany(ns => GetNamedTypes(ns, cancellationToken, predicate))
			)
			.ToImmutableArray();
	}

	private static bool FilterNonSystemNamedTypes(INamedTypeSymbol namedTypeSymbol)
	{
		return namedTypeSymbol.Kind == SymbolKind.NamedType;
	}
}
