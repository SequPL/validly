namespace Valigator.SourceGenerator.Utils.Mapping;

[Flags]
public enum ReturnTypeType
{
	Void = 0,

	Task = 1,

	ValueTask = 1 << 1,

	Enumerable = 1 << 2,

	AsyncEnumerable = 1 << 3,

	ValidationResult = 1 << 4,

	ValidationMessage = 1 << 5,

	Validation = 1 << 6,

	TaskOrValueTask = Task | ValueTask,

	Async = Task | ValueTask,

	Awaitable = Async | AsyncEnumerable,
}
