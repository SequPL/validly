using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if a string matches a specified regular expression pattern.
/// </summary>
[Validator]
[ValidatorDescription("must match the required format")]
[AttributeUsage(AttributeTargets.Property)]
public class MatchAttribute : Attribute
{
	private readonly Regex _regex;

	/// <param name="pattern">The regular expression pattern to match.</param>
	public MatchAttribute([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
	{
		_regex = new Regex(pattern, RegexOptions.Compiled);
	}

	/// <summary>
	/// Validate the value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is string strValue && !_regex.IsMatch(strValue))
		{
			yield return new ValidationMessage("Must match the required format.", "Valigator.Validations.Match");
		}
	}
}
