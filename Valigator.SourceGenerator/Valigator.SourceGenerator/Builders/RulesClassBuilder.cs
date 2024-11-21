using System.Text.RegularExpressions;
using Valigator.SourceGenerator.Dtos;
using Valigator.SourceGenerator.Utils.Mapping;

namespace Valigator.SourceGenerator.Builders;

internal class RulesClassBuilder
{
	private static readonly string RulesDelimiter = $"{Environment.NewLine}{Environment.NewLine}";
	private static readonly string ParameterDelimiter = $",{Environment.NewLine}\t";

	private readonly string _className;
	private readonly List<string> _rules = new();

	public RulesClassBuilder(string className)
	{
		_className = className;
	}

	public void AddRuleForProperty(
		PropertyProperties properties,
		IList<(AttributeProperties, ValidatorProperties)> validators
	)
	{
		if (validators.Count == 0)
		{
			return;
		}

		var tupleTypeArguments = new List<string>();
		var validatorInstances = new List<string>();

		foreach ((AttributeProperties attribute, ValidatorProperties validator) in validators)
		{
			// Tuple type argument
			tupleTypeArguments.Add($"global::{validator.QualifiedName}");

			// Creation of validator instance
			validatorInstances.Add($"new global::{validator.QualifiedName}({string.Join(", ", attribute.Arguments)})");
		}

		// Add null if there is only one validator; just because of tuple syntax - bracket syntax requires more than one element
		if (validatorInstances.Count == 1)
		{
			tupleTypeArguments.Add("object");
			validatorInstances.Add("null");
		}

		_rules.Add(
			$$"""
			internal static readonly ValueTuple<
				{{string.Join(ParameterDelimiter, tupleTypeArguments)}}
			> {{properties.PropertyName}}Rule = (
				{{string.Join(ParameterDelimiter, validatorInstances)}}
			);
			"""
		);
	}

	public string Build(int indent = 0)
	{
		var rules = Regex.Replace(string.Join(RulesDelimiter, _rules), $"^", $"\t", RegexOptions.Multiline);

		var klass = $$"""
			file static class {{_className}}
			{
			{{rules}}
			}
			""";

		return Regex.Replace(klass, "^", new string('\t', indent), RegexOptions.Multiline);
	}
}
