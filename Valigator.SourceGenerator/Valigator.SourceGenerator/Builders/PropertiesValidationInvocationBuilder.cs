using System.Text.RegularExpressions;
using Valigator.SourceGenerator.Dtos;
using Valigator.SourceGenerator.Utils.Mapping;
using Valigator.SourceGenerator.Utils.SourceTexts;

namespace Valigator.SourceGenerator.Builders;

internal class PropertiesValidationInvocationBuilder
{
	private static readonly string InvocationsDelimiter = $"{Environment.NewLine}{Environment.NewLine}\t";
	private static readonly string CallDelimiter = $",{Environment.NewLine}\t";

	private readonly string _ruleClassName;
	private readonly ValigatorConfiguration _config;
	private readonly DependenciesTracker _dependenciesTracker;

	// private Dictionary<string, MethodProperties> _validatableObjectMethods = null!;

	private readonly List<string> _invocations = new();

	/// <summary>
	/// List of validation calls for the custom validation methods.
	/// </summary>
	public CallsCollection Calls { get; } = new();

	public PropertiesValidationInvocationBuilder(
		string ruleClassName,
		ValigatorConfiguration config,
		DependenciesTracker dependenciesTracker
	)
	{
		_ruleClassName = ruleClassName;
		_config = config;
		_dependenciesTracker = dependenciesTracker;
	}

	// public bool HasAnyMethod() => _customValidationMethods.Count > 0;
	//
	// public CustomValidationInterfaceBuilder WithMethods(Dictionary<string, MethodProperties> methods)
	// {
	// 	_validatableObjectMethods = methods;
	// 	return this;
	// }

	public void AddInvocationForProperty(
		PropertyProperties properties,
		IList<(AttributeProperties, ValidatorProperties)> validators,
		CallsCollection propertyCalls
	)
	{
		if (validators.Count == 0)
		{
			return;
		}

		// var propertyCalls = new ValidatorsCallBuilder();

		string ruleName = $"{properties.PropertyName}Rule";
		int itemNumber = 1;

		foreach ((AttributeProperties _, ValidatorProperties validator) in validators)
		{
			// Add CALL
			var arguments = string.Join(
				", ",
				new[] { properties.PropertyName }.Concat(
					validator.IsValidMethod.Dependencies.Select(service => $"service{service}")
				)
			);

			// > XxxRules.NameRule.Item1.IsValid(XyzProp, serviceSomeService)
			propertyCalls.AddValidatorCall(
				$"{_ruleClassName}.{ruleName}.Item{itemNumber}.IsValid({arguments})",
				validator.IsValidMethod.ReturnTypeType
			);

			// Calls.AddValidatorCall(
			// 	$"customValidator.Validate{properties.PropertyName}({arguments})",
			// 	existingMethod.ReturnTypeType
			// );

			itemNumber++;
		}

		// // Generate INVOCATION `xxRule.IsValid()`
		// var validationCalls = new StringBuilder();
		// validationCalls.Append(
		// 	@$"	// Validate {properties.PropertyName}
		// 	context.SetProperty(""{properties.PropertyName}"");
		// 	result.AddPropertyResult(
		// 		{Consts.PropertyValidationResultGlobalRef}.Create(
		// 			""{properties.PropertyName}"""
		// );
		//
		// if (enumerableValidatorInvocationValidatorsLines.Count != 0)
		// {
		// 	validationCalls.Append(
		// 		@$",
		// 			{CreateConcatenation(enumerableValidatorInvocationValidatorsLines)}"
		// 	);
		// }
		//
		// validationCalls.AppendLine(
		// 	@$"
		// 		)
		// 		{CreateAddChain(singleMessageValidatorInvocationValidatorsLines)}
		// 	);"
		// );

		// TODO: Zpracovat všechny typy volání
		// propertyCalls.Messages;
		// propertyCalls.Validations;
		// propertyCalls.Enumerables;
		// propertyCalls.Tasks;
		// propertyCalls.AsyncEnumerables;

		var earlyExitCheck = _config.ExitEarly
			? """

				// if (propertyResult)

				"""
			: string.Empty;

		_invocations.Add(
			$$"""
			// Validate {{properties.PropertyName}} property
			context.SetProperty("{{properties.PropertyName}}");
			propertyResult = {{Consts.PropertyValidationResultGlobalRef}}.Create("{{properties.PropertyName}}");

			{{string.Join(
				CallDelimiter,
				propertyCalls.Messages.Select(call => $"propertyResult.AddValidationMessage({call})")
			)}}
			{{earlyExitCheck}}

			result.AddPropertyResult(propertyResult);
			"""
		);
	}

	public string Build(int indent = 0)
	{
		if (_invocations.Count == 0)
		{
			return string.Empty;
		}

		var code = $$"""
			PropertyValidationResult propertyResult;
			{{string.Join(InvocationsDelimiter, _invocations)}}
			""";

		return Regex.Replace(code, "^", new string('\t', indent), RegexOptions.Multiline);
	}
}
