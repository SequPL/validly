namespace Validly.Tests;

[Validatable(UseAutoValidators = true)]
public partial class AutoValidatorsTestObject
{
	public string Foo { get; init; } = string.Empty;
	public required string Bar { get; init; }
	public required string? Baz { get; init; }
}

public class AutoValidatorsTests
{
	[Fact]
	public void Required_EmptyAllowed()
	{
		var obj = new AutoValidatorsTestObject
		{
			Foo = "",
			Bar = "",
			Baz = "",
		};

		using var result = obj.Validate();

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public void Required_NullFails()
	{
		var obj = new AutoValidatorsTestObject
		{
			Foo = null!,
			Bar = null!,
			Baz = null!,
		};

		using var result = obj.Validate();

		Assert.False(result.IsSuccess);

		// All non-nullable properties failed
		Assert.Collection(
			result.Properties,
			propertyResult => Assert.True(propertyResult.Messages.Count == 1),
			propertyResult => Assert.True(propertyResult.Messages.Count == 1)
		);
	}
}
