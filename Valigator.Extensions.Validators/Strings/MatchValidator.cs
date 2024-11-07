using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Valigator.Validators;

namespace Valigator.Extensions.Validators.Strings;

/// <summary>
/// Validator that checks if a string matches a specified regular expression pattern.
/// </summary>
[Validator]
[ValidationAttribute(typeof(MatchAttribute))]
[ValidatorDescription("must match the required format")]
public class MatchValidator : Validator
{
	private readonly Regex _regex;

	/// <param name="pattern">The regular expression pattern to match.</param>
	public MatchValidator([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
	{
		_regex = new Regex(pattern, RegexOptions.Compiled);
	}

	/// <inheritdoc />
	public override IEnumerable<ValidationMessage> IsValid(object? value)
	{
		if (value is string strValue && !_regex.IsMatch(strValue))
		{
			yield return new ValidationMessage("Must match the required format.", "Valigator.Validations.Match");
		}
	}
}

#pragma warning disable CS1591
public partial class MatchAttribute;
#pragma warning restore CS1591
