using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Benchmarks.Dev;

Job job = Job
	.Default.WithMinWarmupCount(2)
	.WithMaxWarmupCount(4)
	.WithMinIterationCount(5)
	.WithMaxIterationCount(10)
	.WithToolchain(InProcessEmitToolchain.Instance);

IConfig config = DefaultConfig
	.Instance.AddDiagnoser(MemoryDiagnoser.Default)
	.WithOptions(ConfigOptions.DisableOptimizationsValidator)
	.AddJob(job);

// RUN
BenchmarkRunner.Run<DevValigatorBenchmark>(config);
