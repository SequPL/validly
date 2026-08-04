using Validly.Extensions.Validators.Common;

namespace Validly.Tests;

public partial record ValidationContainer
{
	[Validatable]
	public partial record Inputs
	{
		[Required]
		public object[]? Items { get; init; }
	}
}

public class NestedValidatableTypeTests
{
	[Fact]
	public void NestedType_WithValidProperty_IsValid()
	{
		using var result = new ValidationContainer.Inputs { Items = [] }.Validate();

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public void NestedType_WithInvalidProperty_IsInvalid()
	{
		using var result = new ValidationContainer.Inputs { Items = null }.Validate();

		Assert.False(result.IsSuccess);
	}
}
