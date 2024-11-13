using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Benchmarks.Dtos;

namespace Benchmarks;

// [SimpleJob(RuntimeMoniker.Net472, baseline: true)]
// [SimpleJob(RuntimeMoniker.NetCoreApp31)]
// [SimpleJob(RuntimeMoniker.Net70, baseline: true)]
// [SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.NativeAot80)]
// [RPlotExporter]
[MemoryDiagnoser]
public class ValidatorsComparisonWithValidItems
{
	private static readonly ValigatorCreateUserRequest ValigatorCreateUserRequest =
		new()
		{
			Username = "username",
			Password = "S0m3_pa55w0rd#",
			Email = "email@gmail.com",
			Age = 25,
			FirstName = "Tony",
			LastName = "Stark",
		};

	private static readonly DataAnnotationValigatorCreateUserRequest DataAnnotationCreateUserRequest =
		new()
		{
			Username = "username",
			Password = "S0m3_pa55w0rd#",
			Email = "email@gmail.com",
			Age = 25,
			FirstName = "Tony",
			LastName = "Stark",
		};

	[GlobalSetup]
	public void Setup() { }

	[Benchmark]
	public bool Valigator()
	{
		using var result = ValigatorCreateUserRequest.Validate();
		return result.Success;
	}

	[Benchmark]
	public bool DataAnnotation() =>
		Validator.TryValidateObject(
			DataAnnotationCreateUserRequest,
			new ValidationContext(DataAnnotationCreateUserRequest),
			null
		);

	// [Benchmark]
	// public bool Validot() { }

	// [Benchmark]
	// public bool FluentValidation() => md5.ComputeHash(data);
}
