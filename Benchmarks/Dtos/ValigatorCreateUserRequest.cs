using Valigator;
using Valigator.Extensions.Validators.Common;
using Valigator.Extensions.Validators.Numbers;
using Valigator.Extensions.Validators.Strings;

namespace Benchmarks.Dtos;

[Validatable]
public partial class ValigatorCreateUserRequest
{
	[Required]
	[LengthBetween(5, 20)]
	public required string Username { get; set; }

	[Required]
	[MinLength(12)]
	public required string Password { get; set; }

	[Required]
	[EmailAddress]
	public required string Email { get; set; }

	[Between(18, 99)]
	public required int Age { get; set; }

	[NotEmpty]
	public string? FirstName { get; set; }

	[NotEmpty]
	public string? LastName { get; set; }
}
