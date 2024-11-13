using System.Xml;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Valigator.SourceGenerator.ValueProviders;

internal static class ConfigOptionsProvider
{
	private static readonly XmlSerializer Serializer = new(typeof(RawValigatorConfiguration));

	public static IncrementalValueProvider<ValigatorConfiguration> Get(
		IncrementalGeneratorInitializationContext initContext
	)
	{
		return initContext
			.AnalyzerConfigOptionsProvider.Select(ReadRawConfigurationOptions)
			.Select(ToValigatorConfiguration);
	}

	private static string? ReadRawConfigurationOptions(
		AnalyzerConfigOptionsProvider provider,
		CancellationToken cancellationToken
	)
	{
		return provider.GlobalOptions.TryGetValue("build_property.ValigatorConfiguration", out string? configuration)
			? configuration
			: null;
	}

	private static ValigatorConfiguration ToValigatorConfiguration(
		string? configurationXml,
		CancellationToken cancellationToken
	)
	{
		RawValigatorConfiguration raw = Read(configurationXml);

		return new ValigatorConfiguration { AutoRequired = ToBool(raw.AutoRequired) };
	}

	private static RawValigatorConfiguration Read(string? configuration)
	{
		try
		{
			if (configuration != null)
			{
				return Deserialize(configuration);
			}
		}
		catch
		{
			// Ignore
		}

		return new() { AutoRequired = "enable" };
	}

	private static RawValigatorConfiguration Deserialize(string configuration)
	{
		using var stringReader = new StringReader(
			$"<RawValigatorConfiguration>{configuration}</RawValigatorConfiguration>"
		);
		using var reader = new XmlTextReader(stringReader);
		return (RawValigatorConfiguration)Serializer.Deserialize(reader);
	}

	private static bool ToBool(string? value)
	{
		return value is not null
			&& (
				value.Equals("enable", StringComparison.OrdinalIgnoreCase)
				|| value.Equals("enabled", StringComparison.OrdinalIgnoreCase)
				|| value.Equals("true", StringComparison.OrdinalIgnoreCase)
			);
	}
}

public class RawValigatorConfiguration
{
	/// <summary>
	/// When enabled, the Required validator is automatically added to all properties that are not nullable
	/// </summary>
	public string? AutoRequired { get; set; }
}