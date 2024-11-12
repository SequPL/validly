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

		return new MethodProperties
		{
			MethodName = methodSymbol.Name,
			ReturnType =
				qualifiedReturnTypeName && methodSymbol.ReturnType is INamedTypeSymbol namedReturnType
					? namedReturnType.GetQualifiedName()
					: methodSymbol.ReturnType.Name,
			ReturnTypeGenericArgument = methodSymbol.ReturnType is INamedTypeSymbol namedReturnTypeGeneric
				? namedReturnTypeGeneric.TypeArguments.FirstOrDefault()?.Name
				: null,
			Dependencies = new EquatableArray<string>(dependencies),
			IsAsync = methodSymbol.ReturnType.Name is "Task" or "ValueTask" or "AsyncEnumerable",
			Awaitable = methodSymbol.ReturnType.Name is "Task" or "ValueTask",
			RequiresInjection = requiresInjection,
		};
	}

	public static ValidatablePropertyProperties MapValidatableProperty(IPropertySymbol propertySymbol)
	{
		return new ValidatablePropertyProperties
		{
			PropertyName = propertySymbol.Name,
			PropertyType = propertySymbol.Type.Name,
			PropertyIsOfValidatableType = propertySymbol
				.Type.GetAttributes()
				.Any(attr => attr.AttributeClass?.GetQualifiedName() == Consts.ValidatableAttributeQualifiedName),
			ValidationAttributes = new EquatableArray<AttributeProperties>(
				propertySymbol.GetAttributes().Select(MapAttribute).ToArray()
			),
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
}
