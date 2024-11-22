using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
// [SimpleJob(RuntimeMoniker.NetCoreApp31)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.NativeAot80)]
[MaxIterationCount(20)]
// [RPlotExporter]
[MemoryDiagnoser]
public class SimpleValidationLibrariesComparisonBenchmark
{
	[ParamsSource(nameof(Objects))]
	public CreateUserRequest NumberOfInvalidValues { get; set; } = null!;

	public IEnumerable<CreateUserRequest> Objects =>
		new CreateUserRequest[]
		{
			new()
			{
				Username = "username",
				Password = "S0m3_pa55w0rd#",
				Email = "email@gmail.com",
				Age = 25,
				FirstName = "Tony",
				LastName = "Stark",
				NumberOfInvalidItems = "none",
			},
			new()
			{
				Username = "",
				Password = "S0m3_pa55w0rd#",
				Email = "email@gmail.com",
				Age = 25,
				FirstName = "Tony",
				LastName = "Stark",
				NumberOfInvalidItems = "one",
			},
			new()
			{
				Username = "Tom",
				Password = "pass",
				Email = "email[at]gmail.com",
				Age = 16,
				FirstName = "",
				LastName = "",
				NumberOfInvalidItems = "all",
			},
		};

	// [GlobalSetup]
	// public void Setup() { }

	[Benchmark]
	public bool Valigator()
	{
		using var result = NumberOfInvalidValues.Validate();
		return result.IsSuccess;
	}

	[Benchmark]
	public bool DataAnnotation() =>
		Validator.TryValidateObject(NumberOfInvalidValues, new ValidationContext(NumberOfInvalidValues), null);

	// [Benchmark]
	// public bool Validot() { }

	[Benchmark]
	public bool FluentValidation()
	{
		var validator = new CreateUserRequestValidator();
		return validator.Validate(NumberOfInvalidValues).IsValid;
	}
}
