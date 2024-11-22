using Valigator;
using Valigator.Extensions.Validators.Common;
using Valigator.Extensions.Validators.Numbers;
using Valigator.Extensions.Validators.Strings;

namespace Benchmarks;

[Validatable]
public partial class CreateUserRequest
{
	[Required]
	[LengthBetween(5, 20)]
	[System.ComponentModel.DataAnnotations.Required]
	[System.ComponentModel.DataAnnotations.StringLength(20, MinimumLength = 5)]
	public required string Username { get; set; }

	[Required]
	[MinLength(12)]
	[System.ComponentModel.DataAnnotations.Required]
	[System.ComponentModel.DataAnnotations.MinLength(12)]
	public required string Password { get; set; }

	[Required]
	[EmailAddress]
	[System.ComponentModel.DataAnnotations.Required]
	[System.ComponentModel.DataAnnotations.EmailAddress]
	public required string Email { get; set; }

	[Between(18, 99)]
	[System.ComponentModel.DataAnnotations.Range(18, 99)]
	public required int Age { get; set; }

	[NotEmpty]
	[System.ComponentModel.DataAnnotations.MinLength(1)]
	public string? FirstName { get; set; }

	[NotEmpty]
	[System.ComponentModel.DataAnnotations.MinLength(1)]
	public string? LastName { get; set; }

	public required string NumberOfInvalidItems { get; set; }

	public override string ToString()
	{
		return NumberOfInvalidItems;
	}
}
