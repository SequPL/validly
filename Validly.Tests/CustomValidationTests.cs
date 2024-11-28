using Validly.Validators;

namespace Validly.Tests;

[Validatable]
partial class CustomValidationDirectValidationMessage
{
	[CustomValidation]
	public required string SomeValue { get; set; }

	public ValidationMessage? ValidateSomeValue()
	{
		return CustomValidationTests.Message;
	}
}

[Validatable]
partial class CustomValidationEnumerableReturnType
{
	[CustomValidation]
	public required string SomeValue { get; set; }

	public IEnumerable<ValidationMessage> ValidateSomeValue()
	{
		yield return CustomValidationTests.Message;
	}
}

[Validatable(UseAutoValidators = true)]
partial class CustomValidationAsyncEnumerableReturnType
{
	[CustomValidation]
	public required string SomeValue { get; set; }

	public async IAsyncEnumerable<ValidationMessage> ValidateSomeValue()
	{
		await Task.Delay(20);
		yield return AfterValidateTests.Message;
	}
}

public class CustomValidationTests
{
	public static readonly ValidationMessage Message = new("Test error", "message.key");

	// [Fact]
	// public void EnumerableReturnTypeTest()
	// {
	// 	var request = new CustomValidationTestObject { SomeValue = "test" };
	//
	// 	using var result = request.Validate();
	// 	Assert.False(result.IsSuccess);
	// 	Assert.Collection(result.Properties, propertyResult => Assert.True(propertyResult.Messages.Count == 1));
	// }

	[Fact]
	public void DirectValidationResult_Test()
	{
		var request = new CustomValidationDirectValidationMessage { SomeValue = "test" };

		using var result = request.Validate();
		Assert.False(result.IsSuccess);
		Assert.Collection(result.Properties, propertyResult => Assert.True(propertyResult.Messages.Count == 1));
	}

	[Fact]
	public void EnumerableReturnType_Test()
	{
		var request = new CustomValidationEnumerableReturnType { SomeValue = "test" };

		using var result = request.Validate();
		Assert.False(result.IsSuccess);
		Assert.Collection(result.Properties, propertyResult => Assert.True(propertyResult.Messages.Count == 1));
	}

	[Fact]
	public async Task AsyncEnumerableReturnType_Test()
	{
		var request = new CustomValidationAsyncEnumerableReturnType { SomeValue = "test" };

		using var result = await request.ValidateAsync();
		Assert.False(result.IsSuccess);
		Assert.Collection(result.Properties, propertyResult => Assert.True(propertyResult.Messages.Count == 1));
	}
}
