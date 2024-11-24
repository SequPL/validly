using BenchmarkDotNet.Attributes;

namespace Benchmarks.Dev;

public class DevValidlyBenchmark
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

	[Benchmark]
	public bool Validly()
	{
		using var result = NumberOfInvalidValues.Validate();
		return result.IsSuccess;
	}
}
