namespace Valigator.Tests;

[Validatable]
partial class BeforeValidateDirectValidationResult
{
	public required string SomeValue { get; set; }

	private ValidationResult BeforeValidate()
	{
		return new ValidationResult([BeforeValidateTests.Message]);
	}
}

[Validatable]
partial class BeforeValidateEnumerableReturnType
{
	public required string SomeValue { get; set; }

	private IEnumerable<ValidationMessage> BeforeValidate()
	{
		yield return BeforeValidateTests.Message;
	}
}

[Validatable]
partial class BeforeValidateAsyncEnumerableReturnType
{
	public required string SomeValue { get; set; }

	private async IAsyncEnumerable<ValidationMessage> BeforeValidate()
	{
		await Task.Delay(10);
		yield return BeforeValidateTests.Message;
	}
}

public class BeforeValidateTests
{
	public static readonly ValidationMessage Message = new("Test error", "message.key");

	[Fact]
	public void DirectValidationResult_Test()
	{
		var request = new BeforeValidateEnumerableReturnType { SomeValue = "test" };

		using var result = request.Validate();
		Assert.False(result.IsSuccess);
		Assert.Collection(result.Global, message => Assert.Equal(Message, message));
	}

	[Fact]
	public void EnumerableReturnType_Test()
	{
		var request = new BeforeValidateEnumerableReturnType { SomeValue = "test" };

		using var result = request.Validate();
		Assert.False(result.IsSuccess);
		Assert.Collection(result.Global, message => Assert.Equal(Message, message));
	}

	[Fact]
	public async Task AsyncEnumerableReturnType_Test()
	{
		var request = new BeforeValidateAsyncEnumerableReturnType { SomeValue = "test" };

		using var result = await request.Validate();
		Assert.False(result.IsSuccess);
		Assert.Collection(result.Global, message => Assert.Equal(Message, message));
	}
}
