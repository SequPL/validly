using System.Text.RegularExpressions;
using Valigator.SourceGenerator.Dtos;
using Valigator.SourceGenerator.Utils.Mapping;

namespace Valigator.SourceGenerator.Builders;

internal class CustomValidationInterfaceBuilder
{
	private static readonly string MethodDelimiter = $"{Environment.NewLine}{Environment.NewLine}\t";

	// private static readonly string ParameterDelimiter = $",{Environment.NewLine}\t";

	private readonly string _interfaceName;
	private readonly DependenciesTracker _dependenciesTracker;
	private Dictionary<string, MethodProperties> _validatableObjectMethods = null!;
	private readonly List<string> _customValidationMethods = new();

	// public bool ContainsAsyncMethod { get; private set; }

	/// <summary>
	/// List of validation calls for the custom validation methods.
	/// </summary>
	public CallsCollection Calls { get; } = new();

	public CustomValidationInterfaceBuilder(string interfaceName, DependenciesTracker dependenciesTracker)
	{
		_interfaceName = interfaceName;
		_dependenciesTracker = dependenciesTracker;
	}

	public bool HasAnyMethod() => _customValidationMethods.Count > 0;

	public CustomValidationInterfaceBuilder WithMethods(Dictionary<string, MethodProperties> methods)
	{
		_validatableObjectMethods = methods;
		return this;
	}

	public void AddCustomValidationForProperty(PropertyProperties properties, CallsCollection propertyCalls)
	{
		// Generate CUSTOM validation method to the interface

		if (_validatableObjectMethods.TryGetValue($"Validate{properties.PropertyName}", out var existingMethod))
		{
			// // Check if the method is ASYNC
			// if ((existingMethod.ReturnTypeType & ReturnTypeType.Async) != 0)
			// {
			// 	ContainsAsyncMethod = true;
			// }

			// Full return type name including generic arguments
			var nullability = (existingMethod.ReturnTypeType & ReturnTypeType.Nullable) != 0 ? "?" : string.Empty;
			var returnType =
				existingMethod.ReturnType
				+ (
					existingMethod.ReturnTypeGenericArgument is null
						? nullability
						: $"<{existingMethod.ReturnTypeGenericArgument}{nullability}>"
				);

			// Dependencies
			var dependencies = new List<string>();

			foreach (string service in existingMethod.Dependencies)
			{
				// Track the dependency
				_dependenciesTracker.AddDependency(service);

				// Create method parameter
				dependencies.Add($"{service} {service.Substring(0, 1).ToLower() + service.Substring(1)}");
			}

			_customValidationMethods.Add(
				$"""
				/// <summary>
				/// Custom validation method for property '{properties.PropertyName}'
				/// </summary>
				{returnType} Validate{properties.PropertyName}({string.Join(", ", dependencies)});
				"""
			);

			// Add CALL
			var arguments = string.Join(", ", existingMethod.Dependencies.Select(service => $"service{service}"));
			var call = $"customValidator.Validate{properties.PropertyName}({arguments})";

			Calls.AddValidatorCall(call, existingMethod.ReturnTypeType);
			propertyCalls.AddValidatorCall(call, existingMethod.ReturnTypeType);

			// customValidationMethodsForInterface
			// 	.AppendLine(
			// 		$"/// <summary>Custom validation method for property '{properties.PropertyName}'.</summary>"
			// 	)
			// 	.Append($"{existingMethod.ReturnType}")
			// 	.AppendIf(
			// 		$"<{existingMethod.ReturnTypeGenericArgument}>",
			// 		existingMethod.ReturnTypeGenericArgument is not null
			// 	)
			// 	.Append($" Validate{properties.PropertyName}(");
			//
			// customValidationMethodsForInterface.Append(string.Join(", ", dependencies));

			// // Add INVOCATION
			// var customValidatorInvocation =
			// 	$"customValidator.Validate{properties.PropertyName}"
			// 	+ $"({string.Join(", ", existingMethod.Dependencies.Select(service => $"service{service}"))})";

			// if (existingMethod.ReturnType == Consts.ValidationMessageName)
			// {
			// 	singleMessageValidatorInvocationValidatorsLines.Add(
			// 		(customValidatorInvocation, "Custom property validator")
			// 	);
			// }
			// else
			// {
			// 	enumerableValidatorInvocationValidatorsLines.Add(customValidatorInvocation);
			// }
		}
		else
		{
			_customValidationMethods.Add(
				$"""
				/// <summary>
				/// Custom validation method for property '{properties.PropertyName}'
				/// </summary>
				IEnumerable<ValidationMessage> Validate{properties.PropertyName}();
				"""
			);

			// Add CALL
			var call = $"customValidator.Validate{properties.PropertyName}()";
			Calls.AddValidatorCall(call, ReturnTypeType.Enumerable);
			propertyCalls.AddValidatorCall(call, ReturnTypeType.Enumerable);

			// customValidationMethodsForInterface
			// 	.AppendLine(
			// 		$"/// <summary>Custom validation method for property '{properties.PropertyName}'.</summary>"
			// 	)
			// 	.Append($"IEnumerable<ValidationMessage> Validate{properties.PropertyName}(");
			//
			// // Add INVOCATION
			// enumerableValidatorInvocationValidatorsLines.Add($"customValidator.Validate{properties.PropertyName}()");
		}

		// customValidationMethodsForInterface.AppendLine(");");

		//
		//
		//


		// 		var tupleTypeArguments = new List<string>();
		// 		var validatorInstances = new List<string>();
		//
		// 		foreach ((AttributeProperties attribute, ValidatorProperties validator) in validators)
		// 		{
		// 			// Tuple type argument
		// 			tupleTypeArguments.Add($"global::{validator.QualifiedName}");
		//
		// 			// Creation of validator instance
		// 			validatorInstances.Add($"new global::{validator.QualifiedName}({string.Join(", ", attribute.Arguments)})");
		// 		}
		//
		// 		// Add null if there is only one validator; just because of tuple syntax - bracket syntax requires more than one element
		// 		if (validatorInstances.Count == 1)
		// 		{
		// 			validatorInstances.Add("null");
		// 		}
		//
		// 		_customValidationMethods.Add(
		// 			$$"""
		// 			internal static readonly ValueTuple<{{string.Join(
		// 				ParameterDelimiter,
		// 				tupleTypeArguments
		// 			)}}> {{properties.PropertyName}}Rule = (
		// 				{{string.Join(ParameterDelimiter, validatorInstances)}}
		// 			);
		// 			"""
		// 		);
	}

	public string Build()
	{
		if (_customValidationMethods.Count == 0)
		{
			return string.Empty;
		}

		return $$"""
			internal interface {{_interfaceName}}
			{
				{{string.Join(MethodDelimiter, _customValidationMethods)}}
			}
			""";
	}
}
