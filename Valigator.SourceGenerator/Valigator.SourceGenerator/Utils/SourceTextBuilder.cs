using System.Text.RegularExpressions;
using Valigator.SourceGenerator.Utils.FileBuilders;

namespace Valigator.SourceGenerator.Utils;

internal class SourceTextBuilder
{
	private readonly List<string> _usings = new();
	private string? _namespace;

	/// <summary>
	/// Class | Record | Interface
	/// </summary>
	private string _declarationKeyword = "class";

	private readonly List<string> _baseClasses = new();
	private readonly List<string> _interfaces = new();
	private readonly List<FilePart> _memberSources = new();
	private string _name = string.Empty;
	private string _partial = string.Empty;
	private string _accessModifier = "public";
	private string _static = string.Empty;

	private SourceTextBuilder() { }

	public static SourceTextBuilder CreateClassOrRecord(string keyword, string name)
	{
		var builder = new SourceTextBuilder { _name = name, _declarationKeyword = keyword };

		return builder;
	}

	public static SourceTextBuilder CreateInterface(string name)
	{
		var builder = new SourceTextBuilder { _name = name, _declarationKeyword = "interface" };

		return builder;
	}

	public static SourceTextBuilder CreateClass(string name)
	{
		var builder = new SourceTextBuilder { _name = name, _declarationKeyword = "class" };

		return builder;
	}

	public static SourceTextBuilder CreateRecord(string name)
	{
		var builder = new SourceTextBuilder { _name = name, _declarationKeyword = "record" };

		return builder;
	}

	public SourceTextBuilder AddUsings(params string[] usings)
	{
		_usings.AddRange(usings);
		return this;
	}

	public SourceTextBuilder SetNamespace(string @namespace)
	{
		_namespace = @namespace;
		return this;
	}

	public SourceTextBuilder SetAccessModifier(string accessModifier)
	{
		_accessModifier = accessModifier;
		return this;
	}

	public SourceTextBuilder AddBaseClasses(params string[] baseClasses)
	{
		_baseClasses.AddRange(baseClasses);
		return this;
	}

	public SourceTextBuilder AddInterfaces(params string[] interfaces)
	{
		_interfaces.AddRange(interfaces);
		return this;
	}

	public SourceTextBuilder AddMember(FilePart memberSource)
	{
		_memberSources.Add(memberSource);
		return this;
	}

	public SourceTextBuilder Partial()
	{
		_partial = "partial";
		return this;
	}

	public SourceTextBuilder Static()
	{
		_static = "static";
		return this;
	}

	private static readonly Regex MultiWhitespaceReplaceRegex = new(@"\s+", RegexOptions.Compiled);

	public string Build()
	{
		var builder = new FileBuilder();

		// Add USINGs
		AppendUsings(builder);

		var classBuilder = new FileBuilder().AppendLine(
			MultiWhitespaceReplaceRegex.Replace(
				$"{_accessModifier} {_static} {_partial} {_declarationKeyword} {_name}",
				" "
			)
		);

		// Add BASE CLASS and/or INTERFACEs
		AppendBaseTypes(classBuilder);

		// { CLASS body START
		classBuilder.AppendLine("{");

		// Add MEMBERs
		foreach (FilePart memberSource in _memberSources)
		{
			memberSource.Indent();
			classBuilder.AddPart(memberSource);
		}

		classBuilder.AddPart();

		// } CLASS body END
		classBuilder.AppendLine("}");

		// Add class to a namespace
		builder.Concat(WithNamespace(classBuilder));

		return builder.ToString();
	}

	private FileBuilder WithNamespace(FileBuilder code)
	{
		if (string.IsNullOrWhiteSpace(_namespace))
		{
			return code;
		}

		var namespaceBuilder = new FileBuilder().AppendLine($"namespace {_namespace}").AppendLine("{");

		foreach (FilePart part in code.Parts)
		{
			part.Indent();
		}

		namespaceBuilder.Concat(code).AppendLine("}");

		return namespaceBuilder;
	}

	private void AppendBaseTypes(FileBuilder builder)
	{
		if (_baseClasses.Count > 0 || _interfaces.Count > 0)
		{
			builder.Append("\t: ");
		}
		else
		{
			return;
		}

		if (_baseClasses.Count > 0)
		{
			builder.Append(string.Join(", ", _baseClasses));
		}

		if (_interfaces.Count > 0)
		{
			builder.Append(string.Join(", ", _interfaces));
		}

		builder.AppendLine();
	}

	private void AppendUsings(FileBuilder builder)
	{
		if (_usings.Any())
		{
			builder.AppendLine("#pragma warning disable CS0105");
		}

		foreach (var @using in _usings)
		{
			builder.AppendLine(@using.StartsWith("using ") ? @using : $"using {@using};");
		}

		if (_usings.Any())
		{
			builder.AppendLine("#pragma warning restore CS0105");
		}

		builder.AppendLine();
	}
}
