// using System.Collections.Immutable;
// using Microsoft.CodeAnalysis;
//
// namespace Valigator.SourceGenerator.Utils;
//
// internal static class AllSyntaxTreesProvider
// {
// 	public static IncrementalValueProvider<ImmutableArray<SyntaxNode>> GetAllSyntaxTreesProvider(
// 		this IncrementalGeneratorInitializationContext context
// 	)
// 	{
// 		// Create a provider over all the syntax trees in the compilation.  This is better than CreateSyntaxProvider as
// 		// using SyntaxTrees is purely syntax and will not update the incremental node for a tree when another tree is
// 		// changed. CreateSyntaxProvider will have to rerun all incremental nodes since it passes along the
// 		// SemanticModel, and that model is updated whenever any tree changes (since it is tied to the compilation).
// 		var syntaxTreesProvider = context
// 			.CompilationProvider.SelectMany(static (c, _) => c.SyntaxTrees)
// 			.Select(static (st, cancellationToken) => st.GetRoot(cancellationToken))
// 			.Collect()
// 			.WithTrackingName("GetAllSyntaxtrees_syntaxTreesProvider");
//
// 		return syntaxTreesProvider;
// 	}
// }
