using Valigator.SourceGenerator.Utils.Mapping;

namespace Valigator.SourceGenerator.Builders;

internal class CallsCollection
{
	/// <summary>
	/// Calls with return type ValidationMessage; complexity 1
	/// </summary>
	public readonly List<ValidatorCallInfo> Messages = new();

	/// <summary>
	/// Calls with return type Validation; complexity 2 (take more memory than just messages)
	/// </summary>
	public readonly List<ValidatorCallInfo> Validations = new();

	/// <summary>
	/// Calls with return type IEnumerable{T}; complexity 3 (enumerators allocate quite a bit of memory and take some time to iterate)
	/// </summary>
	public readonly List<ValidatorCallInfo> Enumerables = new();

	/// <summary>
	/// Calls with return type Task{T} or ValueTask{T}; complexity 4
	/// </summary>
	public readonly List<ValidatorCallInfo> Tasks = new();

	/// <summary>
	///  with return type IAsyncEnumerable{T}; complexity 5 (like Tasks but with more memory and time)
	/// </summary>
	public readonly List<ValidatorCallInfo> AsyncEnumerables = new();

	public bool AnyAsync() => Tasks.Any() || AsyncEnumerables.Any();

	/// <summary>
	/// Add call to collection
	/// </summary>
	/// <param name="call"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public CallsCollection AddValidatorCall(string call, ReturnTypeType type)
	{
		var validatorCall = new ValidatorCallInfo { Call = call };

		if ((type & ReturnTypeType.TaskOrValueTask) != 0)
		{
			Tasks.Add(validatorCall);
		}
		else if ((type & ReturnTypeType.Enumerable) != 0)
		{
			Enumerables.Add(validatorCall);
		}
		else if ((type & ReturnTypeType.AsyncEnumerable) != 0)
		{
			AsyncEnumerables.Add(validatorCall);
		}
		else if ((type & ReturnTypeType.Validation) != 0)
		{
			Validations.Add(validatorCall);
		}
		else if ((type & ReturnTypeType.ValidationMessage) != 0)
		{
			Messages.Add(validatorCall);
		}

		return this;
	}
}

internal class ValidatorCallInfo
{
	public required string Call { get; init; }
}
