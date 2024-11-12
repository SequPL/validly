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
		SemanticModel semanticModel = context.SemanticModel;
		var symbol = context.TargetSymbol;

		Debug.Assert(context.TargetNode is ClassDeclarationSyntax or RecordDeclarationSyntax);

		if (symbol is not INamedTypeSymbol typeSymbol || context.TargetNode is not TypeDeclarationSyntax targetNode)
		{
			return null;
		}

		var usings = GetUsings(targetNode);
		var propertyDeclarations = targetNode.Members.OfType<PropertyDeclarationSyntax>();

		var membersSymbols = typeSymbol.GetMembers();
		// var beforeValidateMethodSymbol =
		// 	typeSymbol.GetMembers(Consts.BeforeValidateMethodName).FirstOrDefault() as IMethodSymbol;
		// var afterValidateMethodSymbol =
		// 	typeSymbol.GetMembers(Consts.AfterValidateMethodName).FirstOrDefault() as IMethodSymbol;
		var methods = membersSymbols.OfType<IMethodSymbol>().Select(s => SymbolMapper.MapMethod(s)).ToArray();
		var properties = membersSymbols.OfType<IPropertySymbol>().Select(SymbolMapper.MapValidatableProperty).ToArray();

		// var properties = new List<ValidatablePropertyProperties>();
		//
		// foreach (var propertyDeclaration in propertyDeclarations)
		// {
		// 	// var propertySymbol = propertiesSymbols.FirstOrDefault(s =>
		// 	// 	s.Name == propertyDeclaration.Identifier.ValueText
		// 	// );
		//
		// 	if (
		// 		semanticModel.GetDeclaredSymbol(propertyDeclaration, cancellationToken)
		// 		is not IPropertySymbol propertySymbol
		// 	)
		// 	{
		// 		continue;
		// 	}
		//
		// 	var attributesDeclarations = propertyDeclaration
		// 		.AttributeLists.SelectMany(attributeList => attributeList.Attributes)
		// 		.Select(attr => attr.ArgumentList?.Arguments)
		// 		.ToArray();
		//
		// 	var attributes = propertySymbol
		// 		.GetAttributes()
		// 		.Select(
		// 			(attribute, attributeIndex) =>
		// 			{
		// 				return new AttributeProperties(
		// 					attribute.AttributeClass?.ToDisplayString(QualifiedNameArityFormat) ?? string.Empty,
		// 					attributesDeclarations[attributeIndex]?.ToString() ?? string.Empty
		// 				// attribute.ConstructorArguments.Any()
		// 				// 	? string.Join(
		// 				// 		", ",
		// 				// 		attribute.ConstructorArguments.Select(
		// 				// 			(arg, argIndex) =>
		// 				// 			{
		// 				// 				if (
		// 				// 					attribute
		// 				// 						.AttributeConstructor?.Parameters[argIndex]
		// 				// 						.GetAttributes()
		// 				// 						.Any(attr => attr.AttributeClass?.Name == "AsExpressionAttribute") ?? false
		// 				// 				)
		// 				// 				{
		// 				// 					return arg.Value?.ToString();
		// 				// 				}
		// 				//
		// 				// 				return attributesDeclarations[attributeIndex]?[argIndex]?.ToString()
		// 				// 					?? string.Empty;
		// 				// 			}
		// 				// 		)
		// 				// 	)
		// 				// 	: attributesDeclarations[attributeIndex]?.ToString() ?? string.Empty
		// 				);
		// 			}
		// 		)
		// 		.ToArray();
		//
		// 	properties.Add(
		// 		new ValidatablePropertyProperties(
		// 			propertySymbol.Name,
		// 			propertySymbol.Type.Name,
		// 			propertySymbol
		// 				.Type.GetAttributes()
		// 				.Any(attr =>
		// 					attr.AttributeClass?.ToDisplayString(QualifiedNameArityFormat)
		// 					== Consts.ValidatableAttributeQualifiedName
		// 				),
		// 			new EquatableArray<AttributeProperties>(attributes)
		// 		)
		// 	);
		// }

		// var methods = targetNode
		// 	.Members.OfType<MethodDeclarationSyntax>()
		// 	.Where(x =>
		// 		x.Identifier.ValueText is "BeforeValidate" or "AfterValidate"
		// 		// Custom validation methods
		// 		|| x.Identifier.ValueText.StartsWith(Consts.CustomValidationMethodPrefix)
		// 	)
		// 	.ToDictionary(
		// 		method => method.Identifier.ValueText,
		// 		method => new MethodProperties
		// 		{
		// 			MethodName = method.Identifier.ValueText,
		// 			ReturnType = method.ReturnType.ToString(),
		// 			Dependencies = new EquatableArray<string>(
		// 				method.ParameterList.Parameters.Select(x => x.Type?.ToString() ?? "object").ToArray()
		// 			),
		// 		}
		// 	);

		// methods.TryGetValue("BeforeValidate", out var beforeValidateReturnType);
		// methods.TryGetValue("AfterValidate", out var afterValidateReturnType);

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

			// InheritedValidatableObjectRequiresContext =
			// 	inheritsValidatableObject
			// 	&& typeSymbol
			// 		.BaseType!.GetMembers()
			// 		.Any(x =>
			// 			x.Kind is SymbolKind.Method && x.Name == "Validate" /* && x.Met.Any(p => p.Name == "context")*/
			// 		),
			// HasBeforeValidate = beforeValidateReturnType is not null,
			// BeforeValidateReturnType = beforeValidateReturnType?.ReturnType switch
			// {
			// 	"IEnumerable<ValidationMessage>" => BeforeValidateReturnType.Enumerable,
			// 	"IAsyncEnumerable<ValidationMessage>" => BeforeValidateReturnType.AsyncEnumerable,
			// 	"ValidationResult" or "ValidationResult?" => BeforeValidateReturnType.ValidationResult,
			// 	"Task<ValidationResult>"
			// 	or "Task<ValidationResult?>"
			// 	or "ValueTask<ValidationResult>"
			// 	or "ValueTask<ValidationResult?>" => BeforeValidateReturnType.TaskValidationResult,
			// 	"Task" or "ValueTask" => BeforeValidateReturnType.Task,
			// 	_ => BeforeValidateReturnType.Void,
			// },
			// HasAfterValidate = afterValidateReturnType is not null,
			// AfterValidateReturnType = afterValidateReturnType?.ReturnType switch
			// {
			// 	"ValidationResult" or "ValidationResult?" => AfterValidateReturnType.ValidationResult,
			// 	"Task<ValidationResult>" or "ValueTask<ValidationResult>" =>
			// 		AfterValidateReturnType.TaskValidationResult,
			// 	_ => AfterValidateReturnType.ValidationResult,
			// },
		};
	}

	private static UsingDirectiveSyntax[] GetUsings(TypeDeclarationSyntax targetNode)
	{
		var usings = targetNode.Parent?.Parent is CompilationUnitSyntax cus ? cus.Usings.ToArray() : [];
		return usings;
	}
}
