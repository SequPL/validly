using Valigator.SourceGenerator.Utils.Mapping;
using Valigator.SourceGenerator.Utils.SourceTexts.FileBuilders;

namespace Valigator.SourceGenerator.Utils.SourceTexts;

internal class ValidatorsCallBuilder
{
	/// <summary>
	/// Calls with return type ValidationMessage; complexity 1
	/// </summary>
	private readonly List<ValidatorCallInfo> _messages = new();

	/// <summary>
	/// Calls with return type Validation; complexity 2 (take more memory than just messages)
	/// </summary>
	private readonly List<ValidatorCallInfo> _validations = new();

	/// <summary>
	/// Calls with return type IEnumerable{T}; complexity 3 (enumerators allocate quite a bit of memory and take some time to iterate)
	/// </summary>
	private readonly List<ValidatorCallInfo> _enumerables = new();

	/// <summary>
	/// Calls with return type Task{T} or ValueTask{T}; complexity 4
	/// </summary>
	private readonly List<ValidatorCallInfo> _tasks = new();

	/// <summary>
	///  with return type IAsyncEnumerable{T}; complexity 5 (like Tasks but with more memory and time)
	/// </summary>
	private readonly List<ValidatorCallInfo> _asyncEnumerables = new();

	public ValidatorsCallBuilder AddValidatorCall(string call, ReturnTypeType type)
	{
		var validatorCall = new ValidatorCallInfo { Call = call };

		if ((type & ReturnTypeType.TaskOrValueTask) != 0)
		{
			_tasks.Add(validatorCall);
		}
		else if ((type & ReturnTypeType.Enumerable) != 0)
		{
			_enumerables.Add(validatorCall);
		}
		else if ((type & ReturnTypeType.AsyncEnumerable) != 0)
		{
			_asyncEnumerables.Add(validatorCall);
		}
		else if ((type & ReturnTypeType.Validation) != 0)
		{
			_validations.Add(validatorCall);
		}
		else if ((type & ReturnTypeType.ValidationMessage) != 0)
		{
			_messages.Add(validatorCall);
		}

		return this;
	}

	public SourceTextSectionBuilder Build()
	{
		var filePart = new SourceTextSectionBuilder();

		return filePart;
	}
}

internal class ValidatorCallInfo
{
	public required string Call { get; init; }
}
