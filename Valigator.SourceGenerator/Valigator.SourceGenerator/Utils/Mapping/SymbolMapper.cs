using Microsoft.CodeAnalysis;

namespace Valigator.SourceGenerator.Utils.Mapping;

internal static class SymbolMapper
{
	public static MethodProperties MapMethod(
		IMethodSymbol methodSymbol,
		SemanticModel? semanticModel,
		bool skipFirstParameter = false,
		bool qualifiedReturnTypeName = false
	)
	{
		string[] dependencies = new string[
			skipFirstParameter ? Math.Max(methodSymbol.Parameters.Length - 1, 0) : methodSymbol.Parameters.Length
		];

		for (int i = skipFirstParameter ? 1 : 0; i < methodSymbol.Parameters.Length; i++)
		{
			dependencies[i] = methodSymbol.Parameters[i].Type.Name;
		}

		var namedReturnType = methodSymbol.ReturnType as INamedTypeSymbol;

		return new MethodProperties
		{
			MethodName = methodSymbol.Name,
			ReturnType =
				qualifiedReturnTypeName && namedReturnType is not null
					? namedReturnType.GetQualifiedName()
					: methodSymbol.ReturnType.Name,
			ReturnTypeType = namedReturnType is not null
				? ToReturnTypeType(namedReturnType, semanticModel)
				: ReturnTypeType.Void,
			ReturnTypeGenericArgument = namedReturnType?.TypeArguments.FirstOrDefault()?.Name,
			Dependencies = new EquatableArray<string>(dependencies),
		};
	}

	public static PropertyProperties MapValidatableProperty(IPropertySymbol propertySymbol, SemanticModel semanticModel)
	{
		var attributes = propertySymbol.GetAttributes().Select(MapAttribute).ToArray();

		return new PropertyProperties
		{
			PropertyName = propertySymbol.Name,
			DisplayName =
				attributes
					.FirstOrDefault(attr => attr.QualifiedName == Consts.DisplayNameAttributeQualifiedName)
					?.Arguments.FirstOrDefault() ?? propertySymbol.Name,
			PropertyType = propertySymbol.Type.Name,
			PropertyTypeKind = propertySymbol.Type.TypeKind,
			Nullable =
				propertySymbol.NullableAnnotation == NullableAnnotation.Annotated
				|| (
					semanticModel.GetNullableContext(propertySymbol.Locations[0].SourceSpan.Start)
					& NullableContext.Enabled
				) != NullableContext.Enabled,
			PropertyIsOfValidatableType = propertySymbol
				.Type.GetAttributes()
				.Any(attr => attr.AttributeClass?.GetQualifiedName() == Consts.ValidatableAttributeQualifiedName),
			ValidationAttributes = new EquatableArray<AttributeProperties>(attributes),
		};
	}

	private static AttributeProperties MapAttribute(AttributeData arg)
	{
		return new AttributeProperties
		{
			QualifiedName = arg.AttributeClass?.GetQualifiedName() ?? string.Empty,
			Arguments = new EquatableArray<string>(
				arg.ConstructorArguments.Select(ValueToCode)
					.Concat(
						arg.NamedArguments.Select(x =>
							$"{x.Key} = {x.Value.Value ?? $"[{string.Join(", ", x.Value.Values)}]"}"
						)
					)
					.ToArray()
			),
		};
	}

	private static string ValueToCode(TypedConstant constant)
	{
		return constant.Kind switch
		{
			TypedConstantKind.Array => $"new[] {{ {string.Join(", ", constant.Values.Select(ValueToCode))} }}",
			TypedConstantKind.Type => $"typeof({constant.Value})",
			TypedConstantKind.Primitive => constant.Value switch
			{
				string s => $"\"{s}\"",
				char c => $"'{c}'",
				bool b => b.ToString().ToLowerInvariant(),
				_ => constant.Value?.ToString() ?? "null",
			},
			_ => constant.Value?.ToString() ?? "null",
		};
	}

	public static ReturnTypeType ToReturnTypeType(INamedTypeSymbol returnType, SemanticModel? semanticModel)
	{
		ReturnTypeType result = returnType.Name switch
		{
			"Task" => ReturnTypeType.Task,
			"ValueTask" => ReturnTypeType.ValueTask,
			"IAsyncEnumerable" => ReturnTypeType.AsyncEnumerable,
			"IEnumerable" => ReturnTypeType.Enumerable,
			"ValidationResult" => ReturnTypeType.ValidationResult,
			"ValidationMessage" => ReturnTypeType.ValidationMessage,
			"Validation" => ReturnTypeType.Validation,
			_ => ReturnTypeType.Void,
		};

		if ((result & ReturnTypeType.MayBeGeneric) != 0)
		{
			result |= returnType.TypeArguments.FirstOrDefault()?.Name switch
			{
				"ValidationResult" => ReturnTypeType.ValidationResult,
				"ValidationMessage" => ReturnTypeType.ValidationMessage,
				"Validation" => ReturnTypeType.Validation,
				_ => ReturnTypeType.Void,
			};
		}

		if (
			returnType.NullableAnnotation == NullableAnnotation.Annotated
			|| (
				semanticModel is not null
				&& (
					semanticModel.GetNullableContext(returnType.Locations[0].SourceSpan.Start) & NullableContext.Enabled
				) != NullableContext.Enabled
			)
		)
		{
			result |= ReturnTypeType.Nullable;
		}

		return result;
	}
}
