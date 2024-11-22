using Valigator.Extensions.Validators.Common;
using Valigator.Extensions.Validators.Numbers;
using Valigator.Extensions.Validators.Strings;
using Valigator.Validators;

namespace Valigator.Tests;

[Validatable]
public partial class CreateUserRequest
{
	[Required]
	[MinLength(2)]
	public required string Name { get; set; }

	[EmailAddress]
	[CustomValidation]
	public required string Email { get; set; }

	[Required]
	[Between(18, 99)]
	public int Age { get; set; }

	// IEnumerable<ValidationMessage> ICreateUserRequestCustomValidation.ValidateEmail()
	// {
	// 	return [];
	// }
	IEnumerable<ValidationMessage> ICreateUserRequestCustomValidation.ValidateEmail()
	{
		// if (Email.Contains("gmail"))
		// 	yield return new ValidationMessage("Email cannot contain 'gmail'", "");
		return [];
	}

	private ValidationResult AfterValidate(ValidationResult result)
	{
		return result;
	}
}

// [Validatable]
// public partial class CreateUserRequest2 : CreateUserRequest { }

public class BaseTests
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

		using var result = request.Validate();

		Assert.True(result.IsSuccess);
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

		using var result = request.Validate();

		// Validation FAILED
		Assert.False(result.IsSuccess);

		// Asset SINGLE error message on Name property
		var nameResult = result.Properties.SingleOrDefault(x => !x.IsSuccess);
		Assert.NotNull(nameResult);
		Assert.Equal(nameof(CreateUserRequest.Name), nameResult.PropertyName);
		Assert.Single(nameResult.Messages);
	}

	[Fact]
	public void AgeFailTest()
	{
		var request = new CreateUserRequest
		{
			Name = "John",
			Email = "john@gmail.com",
			Age = 16,
		};

		using var result = request.Validate();

		// Validation FAILED
		Assert.False(result.IsSuccess);

		// Asset SINGLE error message on Name property
		var ageResult = result.Properties.SingleOrDefault(x => !x.IsSuccess);
		Assert.NotNull(ageResult);
		Assert.Equal(nameof(CreateUserRequest.Age), ageResult.PropertyName);
		Assert.Single(ageResult.Messages);
	}

	[Fact]
	public void EmailFailTest()
	{
		var request = new CreateUserRequest
		{
			Name = "John",
			Email = "email:john[at]gmail.com",
			Age = 25,
		};

		using var result = request.Validate();

		// Validation FAILED
		Assert.False(result.IsSuccess);

		// Asset SINGLE error message on Name property
		var emailResult = result.Properties.SingleOrDefault(x => !x.IsSuccess);
		Assert.NotNull(emailResult);
		Assert.Equal(nameof(CreateUserRequest.Email), emailResult.PropertyName);
		Assert.Single(emailResult.Messages);
	}
}
