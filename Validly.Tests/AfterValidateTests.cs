namespace Validly.Tests;

[Validatable]
partial class DirectValidationResult
{
	public required string SomeValue { get; set; }

	private ValidationResult AfterValidate(ExtendableValidationResult result)
	{
		result.AddGlobalMessage(AfterValidateTests.Message);
		return result;
	}
}

[Validatable]
partial class EnumerableReturnType
{
	public required string SomeValue { get; set; }

	private IEnumerable<ValidationMessage> AfterValidate()
	{
		yield return AfterValidateTests.Message;
	}
}

[Validatable]
partial class AsyncEnumerableReturnType
{
	public required string SomeValue { get; set; }

	private async IAsyncEnumerable<ValidationMessage> AfterValidate()
	{
		await Task.Delay(0);
		yield return AfterValidateTests.Message;
	}
}

public class AfterValidateTests
{
	public static readonly ValidationMessage Message = new("Test error", "message.key");

	[Fact]
	public void DirectValidationResult_Test()
	{
		var request = new DirectValidationResult { SomeValue = "test" };

		using var result = request.Validate();
		Assert.False(result.IsSuccess);
		Assert.Collection(result.Global, message => Assert.Equal(Message, message));
	}

	[Fact]
	public void EnumerableReturnType_Test()
	{
		var request = new EnumerableReturnType { SomeValue = "test" };

		using var result = request.Validate();
		Assert.False(result.IsSuccess);
		Assert.Collection(result.Global, message => Assert.Equal(Message, message));
	}

	[Fact]
	public async Task AsyncEnumerableReturnType_Test()
	{
		var request = new AsyncEnumerableReturnType { SomeValue = "test" };

		using var result = await request.Validate();
		Assert.False(result.IsSuccess);
		Assert.Collection(result.Global, message => Assert.Equal(Message, message));
	}
}
