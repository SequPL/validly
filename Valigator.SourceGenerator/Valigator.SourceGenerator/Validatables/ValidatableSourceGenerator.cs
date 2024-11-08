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
	private static readonly SymbolDisplayFormat QualifiedNameArityFormat =
		new(
			globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
			typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces
		);

	public void Initialize(IncrementalGeneratorInitializationContext initContext)
	{
		var validatablesToGenerate = initContext
			.SyntaxProvider.ForAttributeWithMetadataName(
				"Valigator.ValidatableAttribute",
				predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
				transform: static (context, cancellationToken) => GetObjectProperties(context, cancellationToken)
			)
			.WhereNotNull()
			// TODO: Replace SyntaxProvider.ForAttributeWithMetadataName with CompilationProvider
			// .Combine(initContext.CompilationProvider.SelectMany((compilation, cancellationToken) => compilation.SourceModule.ReferencedAssemblySymbols.SelectMany(s => s.GlobalNamespace.GetTypeMembers().Where(t => t.GetAttributes().Any(attr => attr.AttributeClass?.ToDisplayString(QualifiedNameArityFormat) == Consts.ValidatorFullyQualifiedName)))))
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
			// .Select((combined, cancellationToken) => (combined.Right/*, combined.Left.Left*/, combined.Left.Right))
			.Select((combined, cancellationToken) => (combined.Left, combined.Right));

		initContext.RegisterSourceOutput(
			validatablesToGenerate,
			(context, properties) => ExecuteValidatorGeneration(context, properties)
		);
	}

	private ValidatorProperties? GetValidatorProperties(
		GeneratorAttributeSyntaxContext context,
		CancellationToken cancellationToken
	)
	{
		Debug.Assert(context.TargetNode is ClassDeclarationSyntax or RecordDeclarationSyntax);

		if (
			context.TargetSymbol is not INamedTypeSymbol typeSymbol
			|| context.TargetNode is not TypeDeclarationSyntax targetNode
		)
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
			RequiresContext = typeSymbol.AllInterfaces.Any(ifce =>
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

		var properties = new List<PropertyProperties>();

		foreach (var propertyDeclaration in propertyDeclarations)
		{
			var propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclaration, cancellationToken);

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

			properties.Add(
				new PropertyProperties(
					propertyDeclaration.Identifier.ValueText,
					propertyDeclaration.Type.ToString(),
					new EquatableArray<ValidationAttributeProperties>(attributes)
				)
			);
		}

		var methods = targetNode
			.Members.OfType<MethodDeclarationSyntax>()
			.ToDictionary(
				method => method.Identifier.ValueText,
				method => new MethodProperties(
					method.Identifier.ValueText,
					method.ReturnType.ToString(),
					method.ParameterList.Parameters.FirstOrDefault()?.Type?.ToString() == "ValidationContext"
				)
			);

		methods.TryGetValue("BeforeValidate", out var beforeValidateReturnType);

		return new ObjectProperties
		{
			Usings = new EquatableArray<string>(usings.Select(usingSyntax => usingSyntax.ToString()).ToArray()),
			ClassOrRecordKeyword = typeSymbol.IsRecord ? "record" : "class",
			Name = typeSymbol.Name,
			Namespace = typeSymbol.ContainingNamespace.ToString(),
			Properties = new EquatableArray<PropertyProperties>(properties.ToArray()),
			Methods = new EquatableArray<MethodProperties>(methods.Values.ToArray()),
			HasBeforeValidate = beforeValidateReturnType is not null,
			BeforeValidateReturnType = beforeValidateReturnType?.ReturnType switch
			{
				"IEnumerable<ValidationMessage>" => BeforeValidateReturnType.Enumerable,
				"ValidationResult" or "ValidationResult?" => BeforeValidateReturnType.ValidationResult,
				_ => BeforeValidateReturnType.Void,
			},
			HasAfterValidate = methods.ContainsKey("AfterValidate"),
		};
	}

	private static UsingDirectiveSyntax[] GetUsings(TypeDeclarationSyntax targetNode)
	{
		var usings = targetNode.Parent?.Parent is CompilationUnitSyntax cus ? cus.Usings.ToArray() : [];
		return usings;
	}

	private static void ExecuteValidatorGeneration(
		SourceProductionContext context,
		(ObjectProperties Object, ImmutableArray<ValidatorProperties> Validators) properties
	)
	{
		// properties.Validators = context.Compilation.SourceModule.ReferencedAssemblySymbols

		bool hasContext = false;
		bool hasCustomValidation = false;
		string customValidationInterfaceSourceText = string.Empty;

		var validatorRules = new FilePart();
		var ruleValidateCalls = new List<string>();
		var customValidationMethodsForInterface = new FilePart();

		// Name of the class with RULEs
		string rulesClassName = $"{properties.Object.Name}Rules";
		string customValidatorInterfaceName = $"I{properties.Object.Name}CustomValidation";

		// Generate stuff for each property
		foreach (PropertyProperties property in properties.Object.Properties)
		{
			ProcessValidatorProperty(
				property,
				properties.Object,
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
		var validatorRulesClassBuilder = CreateValidatorRulesClassBuilder(
			properties.Object,
			rulesClassName,
			validatorRules
		);

		var validateMethodFilePart = new FilePart()
			.AppendLine("/// <summary>Validate the object.</summary>")
			.Append("public ValidationResult Validate(");

		if (hasContext)
		{
			validateMethodFilePart.Append("ValidationContext context");
		}

		validateMethodFilePart.AppendLine(")").AppendLine("{");

		if (hasCustomValidation)
		{
			validateMethodFilePart
				.AppendLine($"\tvar customValidator = ({customValidatorInterfaceName})this;")
				.AppendLine();
		}

		// BeforeValidate hook
		AppendBeforeValidateHook(properties.Object, validateMethodFilePart);

		validateMethodFilePart
			.AppendLine("\tvar result = new ValidationResult(")
			.AppendLine($"\t\t[]{(ruleValidateCalls.Count == 0 ? string.Empty : ",")}");

		int ruleValidateCallsCount = ruleValidateCalls.Count;
		foreach (string validateCall in ruleValidateCalls)
		{
			ruleValidateCallsCount--;

			validateMethodFilePart.AppendLine(
				$"\t\t{validateCall}{(ruleValidateCallsCount == 0 ? string.Empty : ",")}"
			);
		}
		validateMethodFilePart.AppendLine("\t);").AppendLine();

		// AfterValidate hook
		AppendAfterValidateHook(properties, validateMethodFilePart);

		validateMethodFilePart.AppendLine("\treturn result;");
		validateMethodFilePart.AppendLine("}");

		// Generate the validator part for the original object
		var validatorClassBuilder = SourceTextBuilder
			.CreateClassOrRecord(properties.Object.ClassOrRecordKeyword, properties.Object.Name)
			.Partial()
			.AddUsings(properties.Object.Usings.GetArray() ?? [])
			.SetNamespace(properties.Object.Namespace)
			.AddMember(validateMethodFilePart);

		// If custom validation is used, generate the interface and add it to the validator class
		if (hasCustomValidation)
		{
			var customValidationInterface = SourceTextBuilder
				.CreateInterface(customValidatorInterfaceName)
				.SetAccessModifier("internal")
				.SetNamespace(properties.Object.Namespace);

			// Add MEMBERS
			customValidationInterface.AddMember(customValidationMethodsForInterface);

			// Add the interface on validator part of the original object
			validatorClassBuilder.AddInterfaces(customValidatorInterfaceName);

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

	private static void AppendAfterValidateHook(
		(ObjectProperties Object, ImmutableArray<ValidatorProperties> Validators) properties,
		FilePart validateMethodFilePart
	)
	{
		if (properties.Object.HasAfterValidate)
		{
			validateMethodFilePart.AppendLine("\tresult = AfterValidate(result);").AppendLine();
		}
	}

	private static void AppendBeforeValidateHook(ObjectProperties objectProperties, FilePart validateMethodFilePart)
	{
		if (objectProperties.HasBeforeValidate)
		{
			if (objectProperties.BeforeValidateReturnType == BeforeValidateReturnType.Void)
			{
				validateMethodFilePart.AppendLine("\tBeforeValidate();");
			}
			else
			{
				validateMethodFilePart
					.AppendLine(
						"\tvar beforeResult = global::Valigator.Utils.ValidationResultHelper.ToValidationResult(BeforeValidate());"
					)
					.AppendLine("\tif (beforeResult is not null) return beforeResult;")
					.AppendLine();
			}
		}
	}

	private static SourceTextBuilder CreateValidatorRulesClassBuilder(
		ObjectProperties objectProperties,
		string rulesClassName,
		FilePart validatorRules
	)
	{
		return SourceTextBuilder
			.CreateClass(rulesClassName)
			.Static()
			.SetAccessModifier("file")
			.SetNamespace(objectProperties.Namespace)
			.AddMember(validatorRules);
	}

	private static void ProcessValidatorProperty(
		PropertyProperties property,
		ObjectProperties objectProperties,
		ImmutableArray<ValidatorProperties> validators,
		FilePart validatorRules,
		List<string> ruleValidateCalls,
		FilePart customValidationMethodsForInterface,
		string rulesClassName,
		ref bool hasContext,
		ref bool hasCustomValidation
	)
	{
		bool thisRuleHasCustomValidation = false;
		bool thisHasContext = false;

		validatorRules
			.AppendLine($"internal static readonly PropertyRule<{property.PropertyType}> {property.PropertyName}Rule")
			.AppendLine($"\t= new PropertyRuleBuilder<{property.PropertyType}>(\"{property.PropertyName}\")");

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
				hasContext = thisHasContext = true;
			}

			validatorRules.AppendLine(
				$"\t\t.Use(new global::{validator.Namespace}.{validator.Name}({validationAttribute.Arguments}))"
			);
		}

		validatorRules.AppendLine("\t\t.Build();");

		// Generate invocation `xxRule.Validate()`
		ruleValidateCalls.Add(
			$"{rulesClassName}.{property.PropertyName}Rule.Validate("
				+ $"{property.PropertyName}, "
				+ $"{(thisHasContext ? "context" : "null")}"
				+ (thisRuleHasCustomValidation ? $", customValidator.Validate{property.PropertyName}" : string.Empty)
				+ ")"
		);

		if (thisRuleHasCustomValidation)
		{
			// Generate CUSTOM validation method to the interface
			customValidationMethodsForInterface
				.AppendLine($"/// <summary>Custom validation method for property '{property.PropertyName}'.</summary>")
				.Append($"IEnumerable<ValidationMessage> Validate{property.PropertyName}(");

			var existingMethod = objectProperties.Methods.FirstOrDefault(m =>
				m.MethodName == $"Validate{property.PropertyName}"
			);

			if (existingMethod is not null && existingMethod.RequiresContext)
			{
				hasContext = true;
				customValidationMethodsForInterface.Append("global::Valigator.ValidationContext context");
			}

			customValidationMethodsForInterface.AppendLine(");");
		}
	}
}
