using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Valigator.SourceGenerator.Utils;
using Valigator.SourceGenerator.Utils.Mapping;
using Valigator.SourceGenerator.Validatables.Dtos;

namespace Valigator.SourceGenerator.Validatables.ValueProviders;

internal static class ValidatableObjectIncrementalValueProvider
{
	public static IncrementalValuesProvider<ObjectProperties> Get(IncrementalGeneratorInitializationContext initContext)
	{
		return initContext
			.SyntaxProvider.ForAttributeWithMetadataName(
				Consts.ValidatableAttributeQualifiedName,
				predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
				transform: static (context, cancellationToken) => GetObjectProperties(context, cancellationToken)
			)
			.WhereNotNull();
	}

	private static ObjectProperties? GetObjectProperties(
		GeneratorAttributeSyntaxContext context,
		CancellationToken cancellationToken
	)
	{
		var symbol = context.TargetSymbol;

		if (symbol is not INamedTypeSymbol typeSymbol || context.TargetNode is not TypeDeclarationSyntax targetNode)
		{
			return null;
		}

		var usings = GetUsings(targetNode);
		var membersSymbols = typeSymbol.GetMembers();
		var methods = membersSymbols.OfType<IMethodSymbol>().Select(s => SymbolMapper.MapMethod(s)).ToArray();
		var properties = membersSymbols.OfType<IPropertySymbol>().Select(SymbolMapper.MapValidatableProperty).ToArray();

		bool inheritsValidatableObject =
			typeSymbol
				.BaseType?.GetAttributes()
				.Any(attr => attr.AttributeClass?.GetQualifiedName() == Consts.ValidatableAttributeQualifiedName)
			?? false;

		return new ObjectProperties
		{
			Usings = new EquatableArray<string>(usings.Select(usingSyntax => usingSyntax.ToString()).ToArray()),
			ClassOrRecordKeyword = typeSymbol.IsRecord ? "record" : "class",
			Name = typeSymbol.Name,
			Namespace = typeSymbol.ContainingNamespace.ToString(),
			Properties = new EquatableArray<ValidatablePropertyProperties>(properties),
			Methods = new EquatableArray<MethodProperties>(methods),
			InheritsValidatableObject = inheritsValidatableObject,
			BeforeValidateMethod = methods.FirstOrDefault(m => m.MethodName == Consts.BeforeValidateMethodName),
			AfterValidateMethod = methods.FirstOrDefault(m => m.MethodName == Consts.AfterValidateMethodName),
		};
	}

	private static UsingDirectiveSyntax[] GetUsings(TypeDeclarationSyntax targetNode)
	{
		var usings = targetNode.Parent?.Parent is CompilationUnitSyntax cus ? cus.Usings.ToArray() : [];
		return usings;
	}
}
