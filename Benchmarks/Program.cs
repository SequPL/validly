using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Benchmarks;

Job job = Job
	.Default.WithMinWarmupCount(2)
	.WithMaxWarmupCount(4)
	.WithMinIterationCount(5)
	.WithToolchain(InProcessEmitToolchain.Instance);

IConfig config = DefaultConfig
	.Instance.AddDiagnoser(MemoryDiagnoser.Default)
	.WithOptions(ConfigOptions.DisableOptimizationsValidator)
	.AddJob(job);

if (args.Any(arg => arg == "valigator-only"))
{
	job.WithMaxIterationCount(10);

	BenchmarkRunner.Run<DevValigatorBenchmark>(config);
}
else
{
	BenchmarkRunner.Run<SimpleValidationLibrariesComparisonBenchmark>();
}
