using Valigator.Extensions.Validators.Common;
using Valigator.Extensions.Validators.Numbers;
using Valigator.Extensions.Validators.Strings;
using Valigator.Validators;

namespace Valigator.Tests;

public class UnitTest1
{
	[Fact]
	public void ValidateTest()
	{
		var request = new CreateUserRequest
		{
			Name = "John",
			Email = "john@gmail.com",
			Age = 25,
		};

		var result = request.Validate();

		Assert.True(result.Valid);
	}

	[Fact]
	public void NameFailTest()
	{
		var request = new CreateUserRequest
		{
			Name = "J",
			Email = "john@gmail.com",
			Age = 25,
		};

		var result = request.Validate();

		Assert.True(result.Valid);
	}

	[Fact]
	public void AgeFailTest()
	{
		var request = new CreateUserRequest
		{
			Name = "John",
			Email = "john@gmail.com",
			Age = 25,
		};

		var result = request.Validate();

		Assert.True(result.Valid);
	}
}

[Validatable]
public partial class CreateUserRequest
{
	[Required]
	[MinLength(5)]
	public required string Name { get; set; }

	[EmailAddress]
	[CustomValidation]
	public required string Email { get; set; }

	[Between(18, 99)]
	public int Age { get; set; }

	IEnumerable<ValidationMessage> ICreateUserRequestCustomValidation.ValidateEmail()
	{
		return [];
	}
}
