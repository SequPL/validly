using Microsoft.CodeAnalysis;

namespace Valigator.SourceGenerator.Utils.Mapping;

internal static class SymbolMapper
{
	public static MethodProperties MapMethod(IMethodSymbol methodSymbol, bool qualifiedReturnTypeName = false)
	{
		string[] dependencies = new string[Math.Max(methodSymbol.Parameters.Length - 1, 0)];
		bool requiresInjection = false;

		// Skip first; it should always be `object?` - the validated value
		for (int i = 1; i < methodSymbol.Parameters.Length; i++)
		{
			if (Consts.ValidationContextName != (dependencies[i] = methodSymbol.Parameters[i].Type.Name))
			{
				// If these is some dependency other than ValidationContext, we need to inject it
				requiresInjection = true;
			}
		}

		var namedReturnType = methodSymbol.ReturnType as INamedTypeSymbol;

		return new MethodProperties
		{
			MethodName = methodSymbol.Name,
			ReturnType =
				qualifiedReturnTypeName && namedReturnType is not null
					? namedReturnType.GetQualifiedName()
					: methodSymbol.ReturnType.Name,
			ReturnTypeType = namedReturnType is not null ? ToReturnTypeType(namedReturnType) : ReturnTypeType.Void,
			ReturnTypeGenericArgument = namedReturnType?.TypeArguments.FirstOrDefault()?.Name,
			Dependencies = new EquatableArray<string>(dependencies),
			// IsAsync = methodSymbol.ReturnType.Name is "Task" or "ValueTask" or "IAsyncEnumerable",
			// Awaitable = methodSymbol.ReturnType.Name is "Task" or "ValueTask",
			RequiresInjection = requiresInjection,
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

	public static ReturnTypeType ToReturnTypeType(INamedTypeSymbol returnType)
	{
		ReturnTypeType result = ReturnTypeType.Void;

		switch (returnType.Name)
		{
			case "Task":
				result = ReturnTypeType.Task | ReturnTypeType.Async | ReturnTypeType.Awaitable;
				break;
			case "ValueTask":
				result = ReturnTypeType.ValueTask | ReturnTypeType.Async | ReturnTypeType.Awaitable;
				break;
			case "IAsyncEnumerable":
				result = ReturnTypeType.AsyncEnumerable | ReturnTypeType.Awaitable;
				break;
			case "IEnumerable":
				result = ReturnTypeType.Enumerable;
				break;
		}

		switch (returnType.TypeArguments.FirstOrDefault()?.Name)
		{
			case "ValidationResult":
				result |= ReturnTypeType.ValidationResult;
				break;
			case "ValidationMessage":
				result |= ReturnTypeType.ValidationMessage;
				break;
			case "Validation":
				result |= ReturnTypeType.Validation;
				break;
		}

		return result;
	}
}
