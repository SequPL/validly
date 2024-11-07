using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Valigator.SourceGenerator.Utils;
using Valigator.SourceGenerator.Utils.FileBuilders;
using Valigator.SourceGenerator.Validatables.ObjectDtos;
using Valigator.SourceGenerator.Validatables.ValidatorDtos;

namespace Valigator.SourceGenerator.Validatables;

[Generator]
public class ValidatableSourceGenerator : IIncrementalGenerator
{
	private static readonly SymbolDisplayFormat QualifiedNameArityFormat = new SymbolDisplayFormat(
		globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces
	);

	public void Initialize(IncrementalGeneratorInitializationContext initContext)
	{
		// var validatablesToGenerate = initContext
		// 	.SyntaxProvider.ForAttributeWithMetadataName(
		// 		"Valigator.ValidatableAttribute",
		// 		predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
		// 		transform: static (ctx, cancellationToken) =>
		// 			GetObjectProperties(ctx.SemanticModel, ctx.TargetNode, cancellationToken)
		// 	)
		// 	.WhereNotNull();
		// var allSyntaxTrees = initContext.GetAllSyntaxTreesProvider();
		//
		// initContext.Seman

		// var validatablesToGenerate = initContext
		// 	.SyntaxProvider.CreateSyntaxProvider(IsValidatableClassOrRecord, GetObjectProperties)
		// 	.WhereNotNull();

		var validatablesToGenerate = initContext
			.SyntaxProvider.ForAttributeWithMetadataName(
				"Valigator.ValidatableAttribute",
				predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
				transform: static (context, cancellationToken) => GetObjectProperties(context, cancellationToken)
			)
			.WhereNotNull()
			.Combine(
				initContext
					.SyntaxProvider.ForAttributeWithMetadataName(
						Consts.ValidatorFullyQualifiedName,
						predicate: static (node, _) => node is ClassDeclarationSyntax,
						transform: GetValidatorProperties
					)
					.WhereNotNull()
					.Collect()
			)
			// .Combine(
			// 	initContext
			// 		.SyntaxProvider.ForAttributeWithMetadataName(
			// 			"Valigator.ValidatorAttributeAttribute",
			// 			predicate: static (node, _) => node is ClassDeclarationSyntax,
			// 			transform: GetValidatorAttributeProperties
			// 		)
			// 		.WhereNotNull()
			// 		.Collect()
			// )
			.Combine(initContext.CompilationProvider)
			.Select((combined, cancellationToken) => (combined.Right, combined.Left.Left, combined.Left.Right));
		// .Select((combined, cancellationToken) => (combined.Left, combined.Right));

		initContext.RegisterSourceOutput(
			validatablesToGenerate,
			static (sourceProductionContext, objectProperties) =>
				ExecuteValidatorGeneration(objectProperties, sourceProductionContext)
		);
	}

	private ValidatorProperties? GetValidatorProperties(
		GeneratorAttributeSyntaxContext context,
		CancellationToken cancellationToken
	)
	{
		// SemanticModel semanticModel = context.SemanticModel;
		SyntaxNode targetNode = context.TargetNode;
		ISymbol symbol = context.TargetSymbol;

		Debug.Assert(targetNode is ClassDeclarationSyntax or RecordDeclarationSyntax);

		if (symbol is not INamedTypeSymbol typeSymbol)
		// if (semanticModel.GetDeclaredSymbol(targetNode) is not INamedTypeSymbol typeSymbol)
		{
			return null;
		}

		var usings = GetUsings(targetNode);

		// TODO: Constructor access
		// typeSymbol.GetAttributes().FirstOrDefault().AttributeConstructor

		return new ValidatorProperties
		{
			Usings = new EquatableArray<string>(usings.Select(usingSyntax => usingSyntax.ToString()).ToArray()),
			Namespace = typeSymbol.ContainingNamespace.ToString(),
			Name = typeSymbol.Name,
			RequiresContext = typeSymbol.Interfaces.Any(ifce =>
				ifce.ToDisplayString(QualifiedNameArityFormat) == Consts.IContextValidatorFullyQualifiedName
			),
			PairedValidationAttributeFullyQualifiedName =
				typeSymbol
					.GetAttributes()
					.FirstOrDefault(attr =>
						attr.AttributeClass?.ToDisplayString(QualifiedNameArityFormat)
						== Consts.ValidationAttributeAttributeFullyQualifiedName
					)
					?.ConstructorArguments.FirstOrDefault()
					.Value?.ToString() ?? string.Empty,
		};
	}

	// private static ObjectProperties? GetObjectProperties(SemanticModel semanticModel, SyntaxNode targetNode)
	private static ObjectProperties? GetObjectProperties(
		GeneratorAttributeSyntaxContext context,
		// GeneratorSyntaxContext context,
		CancellationToken cancellationToken
	)
	{
		SemanticModel semanticModel = context.SemanticModel;
		SyntaxNode targetNode = context.TargetNode;
		var symbol = context.TargetSymbol;

		Debug.Assert(targetNode is ClassDeclarationSyntax or RecordDeclarationSyntax);

		if (symbol is not INamedTypeSymbol typeSymbol)
		{
			return null;
		}

		var usings = GetUsings(targetNode);
		var propertyDeclarations = GetPropertyDeclarations(targetNode);

		var properties = new List<PropertyProperties>();

		foreach (var propertyDeclaration in propertyDeclarations)
		{
			var propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclaration);

			if (propertySymbol is null)
			{
				continue;
			}

			var attributesDeclarations = propertyDeclaration
				.AttributeLists.SelectMany(attributeList => attributeList.Attributes)
				.Select(attr => attr.ArgumentList?.Arguments)
				.ToArray();

			var attributes = propertySymbol
				.GetAttributes()
				.Select(
					(attribute, attributeIndex) =>
					{
						return new ValidationAttributeProperties(
							attribute.AttributeClass?.ToDisplayString(QualifiedNameArityFormat) ?? string.Empty,
							attributesDeclarations[attributeIndex]?.ToString() ?? string.Empty
						// attribute.ConstructorArguments.Any()
						// 	? string.Join(
						// 		", ",
						// 		attribute.ConstructorArguments.Select(
						// 			(arg, argIndex) =>
						// 			{
						// 				if (
						// 					attribute
						// 						.AttributeConstructor?.Parameters[argIndex]
						// 						.GetAttributes()
						// 						.Any(attr => attr.AttributeClass?.Name == "AsExpressionAttribute") ?? false
						// 				)
						// 				{
						// 					return arg.Value?.ToString();
						// 				}
						//
						// 				return attributesDeclarations[attributeIndex]?[argIndex]?.ToString()
						// 					?? string.Empty;
						// 			}
						// 		)
						// 	)
						// 	: attributesDeclarations[attributeIndex]?.ToString() ?? string.Empty
						);
					}
				)
				.ToArray();

			// var attributes = propertyDeclaration
			// 	.AttributeLists.SelectMany(attributeList => attributeList.Attributes)
			// 	//.Where(attribute =>
			// 	//	semanticModel
			// 	//		.GetTypeInfo(attribute)
			// 	//		.Type?.GetAttributes()
			// 	//		.Any(x =>
			// 	//			x.AttributeClass?.ToDisplayString(QualifiedNameArityFormat)
			// 	//			== Consts.ValidatorFullyQualifiedName
			// 	//		) ?? false
			// 	//)
			// 	.Select(attribute =>
			// 	{
			// 		//var dec = semanticModel.GetTypeInfo(attribute);
			// 		var attributeSymbol = semanticModel.GetDeclaredSymbol(attribute);
			//
			// 		//if (attributeSymbol is null)
			// 		//{
			// 		//	return null;
			// 		//}
			//
			// 		return new ValidationAttributeProperties(
			// 			attributeSymbol?.ToDisplayString(QualifiedNameArityFormat) ?? string.Empty,
			// 			attribute.ArgumentList?.Arguments.ToString() ?? string.Empty
			// 		// attribute
			// 		);
			// 	})
			// 	.WhereNotNull()
			// 	.ToArray();

			// var xx = propertyDeclaration
			// 	.AttributeLists.SelectMany(attributeList => attributeList.Attributes)
			// 	.Select(attribute => new
			// 	{
			// 		attribute,
			// 		attributes = semanticModel.GetTypeInfo(attribute).Type?.GetAttributes().ToList(),
			// 		qualifiedNames = semanticModel
			// 			.GetTypeInfo(attribute)
			// 			.Type?.GetAttributes()
			// 			.Select(x => x.AttributeClass?.ToDisplayString(QualifiedNameArityFormat))
			// 			.ToList(),
			// 	});

			// var validatorAttributes = propertyDeclaration
			// 	.AttributeLists.SelectMany(attributeList => attributeList.Attributes)
			// 	.Where(attribute =>
			// 		semanticModel
			// 			.GetTypeInfo(attribute)
			// 			.Type?.GetAttributes()
			// 			.Any(x =>
			// 				x.AttributeClass?.ToDisplayString(QualifiedNameArityFormat)
			// 				== Consts.ValidatorFullyQualifiedName
			// 			) ?? false
			// 	)
			// 	.Select(attribute =>
			// 	{
			// 		var attributeSymbol = semanticModel.GetDeclaredSymbol(attribute);
			//
			// 		if (attributeSymbol is null)
			// 		{
			// 			return null;
			// 		}
			//
			// 		return new ValidationAttributeProperties(
			// 			attributeSymbol.ToDisplayString(QualifiedNameArityFormat),
			// 			attribute.ArgumentList?.Arguments.ToString() ?? string.Empty
			// 		);
			// 	})
			// 	.WhereNotNull()
			// 	.ToArray();

			properties.Add(
				new PropertyProperties(
					propertyDeclaration.Identifier.ValueText,
					propertyDeclaration.Type.ToString(),
					new EquatableArray<ValidationAttributeProperties>(attributes)
				)
			);
		}

		return new ObjectProperties
		{
			Usings = new EquatableArray<string>(usings.Select(usingSyntax => usingSyntax.ToString()).ToArray()),
			ClassOrRecordKeyword = typeSymbol.IsRecord ? "record" : "class",
			Name = typeSymbol.Name,
			Namespace = typeSymbol.ContainingNamespace.ToString(),
			Properties = new EquatableArray<PropertyProperties>(properties.ToArray()),
		};
	}

	private static IEnumerable<PropertyDeclarationSyntax> GetPropertyDeclarations(SyntaxNode targetNode)
	{
		IEnumerable<PropertyDeclarationSyntax> propertyDeclarations = targetNode
			is ClassDeclarationSyntax classDeclarationSyntax
			? classDeclarationSyntax.Members.OfType<PropertyDeclarationSyntax>()
			: ((RecordDeclarationSyntax)targetNode).Members.OfType<PropertyDeclarationSyntax>();
		return propertyDeclarations;
	}

	private static UsingDirectiveSyntax[] GetUsings(SyntaxNode targetNode)
	{
		var usings = targetNode.Parent?.Parent is CompilationUnitSyntax cus ? cus.Usings.ToArray() : [];
		return usings;
	}

	private static void ExecuteValidatorGeneration(
		(
			Compilation Compilation,
			ObjectProperties Object,
			ImmutableArray<ValidatorProperties> Validators
		// ImmutableArray<ValidatorAttributeProperties> ValidatorsAttributes
		) properties,
		SourceProductionContext context
	)
	{
		// 		if (validatorProperties is null)
		// 		{
		// 			return;
		// 		}
		//
		// 		var constructors = validatorProperties.Ctors.Select(ctor =>
		// 			$"public {validatorProperties.Name}Attribute({ctor}) {{ }}"
		// 		);
		//

		bool hasContext = false;
		bool hasCustomValidation = false;
		string customValidationInterfaceSourceText = string.Empty;

		var validatorRules = new List<string>();
		var ruleValidateCalls = new List<string>();
		var customValidationMethodsForInterface = new List<string>();

		// Name of the class with RULEs
		string rulesClassName = $"{properties.Object.Name}Rules";

		// Generate stuff for each property
		foreach (PropertyProperties property in properties.Object.Properties)
		{
			ProcessValidatorProperty(
				property,
				properties.Compilation,
				properties.Validators,
				validatorRules,
				ruleValidateCalls,
				customValidationMethodsForInterface,
				rulesClassName,
				ref hasContext,
				ref hasCustomValidation
			);
		}

		// Create separated class with rules; so it's not visible in the original object
		var validatorRulesClassBuilder = SourceTextBuilder
			.CreateClass(rulesClassName)
			.Static()
			.SetAccessModifier("file")
			.SetNamespace(properties.Object.Namespace);

		// Generate the validator part for the original object
		var validatorClassBuilder = SourceTextBuilder
			.CreateClassOrRecord(properties.Object.ClassOrRecordKeyword, properties.Object.Name)
			.Partial()
			.AddUsings(properties.Object.Usings.GetArray() ?? [])
			.SetNamespace(properties.Object.Namespace);

		// If custom validation is used, generate the interface and add it to the validator class
		if (hasCustomValidation)
		{
			var customValidationInterface = SourceTextBuilder
				.CreateInterface($"I{properties.Object.Name}CustomValidation")
				.SetAccessModifier("file")
				.SetNamespace(properties.Object.Namespace);

			// Add MEMBERS
			AddCustomValidationMethodsToInterface(customValidationMethodsForInterface, customValidationInterface);

			// Add the interface on validator part of the original object
			validatorClassBuilder.AddInterfaces($"I{properties.Object.Name}CustomValidation");

			customValidationInterfaceSourceText = customValidationInterface.Build();
		}

		var validator =
			"// <auto-generated/>"
			+ Environment.NewLine
			+ Environment.NewLine
			+ validatorClassBuilder.Build()
			+ validatorRulesClassBuilder.Build()
			+ customValidationInterfaceSourceText;

		context.AddSource($"{properties.Object.Name}.Validator.g.cs", SourceText.From(validator, Encoding.UTF8));
	}

	private static void AddCustomValidationMethodsToInterface(
		List<string> customValidationMethodsForInterface,
		SourceTextBuilder customValidationInterface
	)
	{
		var membersFilePart = new FilePart();

		foreach (string customValidationMethod in customValidationMethodsForInterface)
		{
			membersFilePart.AppendLine(customValidationMethod);
		}

		customValidationInterface.AddMember(membersFilePart);
	}

	private static void ProcessValidatorProperty(
		PropertyProperties property,
		Compilation compilation,
		ImmutableArray<ValidatorProperties> validators,
		List<string> validatorRules,
		List<string> ruleValidateCalls,
		List<string> customValidationMethodsForInterface,
		string rulesClassName,
		ref bool hasContext,
		ref bool hasCustomValidation
	)
	{
		bool thisRuleHasCustomValidation = false;

		var ruleBuilder = new StringBuilder()
			.AppendLine($"internal static readonly PropertyRule<{property.PropertyType}> {property.PropertyName}Rule")
			.AppendLine($"\t= new PropertyRuleBuilder<{property.PropertyType}>({property.PropertyName})");

		foreach (var validationAttribute in property.ValidationAttributes)
		{
			// CUSTOM validation
			if (validationAttribute.FullyQualifiedName == Consts.CustomValidationAttribute)
			{
				thisRuleHasCustomValidation = true;
				hasCustomValidation = true;
				continue;
			}

			var validator = validators.FirstOrDefault(validator =>
				validator.PairedValidationAttributeFullyQualifiedName == validationAttribute.FullyQualifiedName
			);

			if (validator is null)
			{
				continue;
			}

			if (validator.RequiresContext)
			{
				hasContext = true;
			}

			ruleBuilder.AppendLine(
				$"\t\t.Use(new global::{validator.Namespace}.{validator.Name}({validationAttribute.Arguments}))"
			);
		}

		validatorRules.Add(ruleBuilder.Append(".Build();").ToString());

		if (thisRuleHasCustomValidation)
		{
			// Generate invocation `xxRule.Validate()`
			ruleValidateCalls.Add(
				$"{rulesClassName}.{property.PropertyName}Rule.Validate("
					+ $"{property.PropertyName}, "
					+ $"{(hasContext ? "context" : "null")}"
					+ (hasCustomValidation ? $", customValidator.Validate{property.PropertyName}" : string.Empty)
					+ ")"
			);

			// Generate CUSTOM validation method to the interface
			customValidationMethodsForInterface.Add(
				$"IEnumerable<ValidationMessage> Validate{property.PropertyName}();"
			);
		}
	}

	// private ValidatorAttributeProperties? GetValidatorAttributeProperties(
	// 	GeneratorAttributeSyntaxContext context,
	// 	CancellationToken cancellationToken
	// )
	// {
	// 	SyntaxNode targetNode = context.TargetNode;
	// 	var symbol = context.TargetSymbol;
	//
	// 	Debug.Assert(targetNode is ClassDeclarationSyntax or RecordDeclarationSyntax);
	//
	// 	if (symbol is not INamedTypeSymbol typeSymbol)
	// 	// if (semanticModel.GetDeclaredSymbol(targetNode) is not INamedTypeSymbol typeSymbol)
	// 	{
	// 		return null;
	// 	}
	//
	// 	var usings = GetUsings(targetNode);
	//
	// 	return new ValidatorAttributeProperties
	// 	{
	// 		Usings = new EquatableArray<string>(usings.Select(usingSyntax => usingSyntax.ToString()).ToArray()),
	// 		Namespace = typeSymbol.ContainingNamespace.ToString(),
	// 		Name = typeSymbol.Name,
	// 	};
	// }

	// static bool IsValidatableClassOrRecord(SyntaxNode syntaxNode, CancellationToken cancellationToken)
	// {
	// 	if (!syntaxNode.IsKind(SyntaxKind.ClassDeclaration) && !syntaxNode.IsKind(SyntaxKind.RecordDeclaration))
	// 	{
	// 		return false;
	// 	}
	//
	// 	var classOrRecordDeclaration = (TypeDeclarationSyntax)syntaxNode;
	//
	// 	return classOrRecordDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword)
	// 		&& classOrRecordDeclaration.AttributeLists.Any(attributeList =>
	// 			attributeList.Attributes.Any(attribute =>
	// 				// TODO: Do semantic check for the attribute
	// 				attribute.Name.ToString().Replace("Attribute", "") == "Validatable"
	// 			)
	// 		);
	// }
}
