using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Benchmarks.Dtos;

namespace Benchmarks;

// [SimpleJob(RuntimeMoniker.Net472, baseline: true)]
// [SimpleJob(RuntimeMoniker.NetCoreApp31)]
[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.NativeAot80)]
[RPlotExporter]
[MemoryDiagnoser]
public class ValidatorsComparisonWithValidItems
{
	private static readonly ValigatorCreateUserRequest valigatorCreateUserRequest =
		new()
		{
			Username = "username",
			Password = "S0m3_pa55w0rd#",
			Email = "email@gmail.com",
			Age = 25,
			FirstName = "Tony",
			LastName = "Stark",
		};

	private static readonly DataAnnotationValigatorCreateUserRequest dataAnnotationCreateUserRequest =
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
	public bool Valigator() => valigatorCreateUserRequest.Validate().Success;

	[Benchmark]
	public bool DataAnnotation() =>
		Validator.TryValidateObject(
			dataAnnotationCreateUserRequest,
			new ValidationContext(dataAnnotationCreateUserRequest),
			null
		);

	// [Benchmark]
	// public bool Validot() { }

	// [Benchmark]
	// public bool FluentValidation() => md5.ComputeHash(data);
}
