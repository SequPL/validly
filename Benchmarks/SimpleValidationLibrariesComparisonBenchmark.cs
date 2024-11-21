using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

// [SimpleJob(RuntimeMoniker.Net472, baseline: true)]
// [SimpleJob(RuntimeMoniker.NetCoreApp31)]
// [SimpleJob(RuntimeMoniker.Net70, baseline: true)]
// [SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.NativeAot80)]
// [RPlotExporter]
[MemoryDiagnoser]
public class SimpleValidationLibrariesComparisonBenchmark
{
	private static readonly CreateUserRequest CreateUserRequestValid =
		new()
		{
			Username = "username",
			Password = "S0m3_pa55w0rd#",
			Email = "email@gmail.com",
			Age = 25,
			FirstName = "Tony",
			LastName = "Stark",
		};

	private static readonly CreateUserRequest CreateUserRequestOneInvalid =
		new()
		{
			Username = "",
			Password = "S0m3_pa55w0rd#",
			Email = "email@gmail.com",
			Age = 25,
			FirstName = "Tony",
			LastName = "Stark",
		};

	private static readonly CreateUserRequest CreateUserRequestAllInvalid =
		new()
		{
			Username = "Tom",
			Password = "pass",
			Email = "email[at]gmail.com",
			Age = 16,
			FirstName = "",
			LastName = "",
		};

	[GlobalSetup]
	public void Setup() { }

	[Benchmark]
	public bool Valigator()
	{
		using var result = CreateUserRequestValid.Validate();
		return result.IsSuccess;
	}

	[Benchmark]
	public bool ValigatorOneInvalid()
	{
		using var result = CreateUserRequestOneInvalid.Validate();
		return result.IsSuccess;
	}

	[Benchmark]
	public bool ValigatorAllInvalid()
	{
		using var result = CreateUserRequestAllInvalid.Validate();
		return result.IsSuccess;
	}

	[Benchmark]
	public bool DataAnnotation() =>
		Validator.TryValidateObject(CreateUserRequestValid, new ValidationContext(CreateUserRequestValid), null);

	[Benchmark]
	public bool DataAnnotationOneInvalid() =>
		Validator.TryValidateObject(
			CreateUserRequestOneInvalid,
			new ValidationContext(CreateUserRequestOneInvalid),
			null
		);

	[Benchmark]
	public bool DataAnnotationAllInvalid() =>
		Validator.TryValidateObject(
			CreateUserRequestAllInvalid,
			new ValidationContext(CreateUserRequestAllInvalid),
			null
		);

	// [Benchmark]
	// public bool Validot() { }

	// [Benchmark]
	// public bool FluentValidation() => md5.ComputeHash(data);
}
