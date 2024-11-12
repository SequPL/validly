using System.ComponentModel.DataAnnotations;

namespace Benchmarks.Dtos;

public class DataAnnotationValigatorCreateUserRequest
{
	[Required]
	[Length(5, 20)]
	public required string Username { get; set; }

	[Required]
	[MinLength(12)]
	public required string Password { get; set; }

	[Required]
	[EmailAddress]
	public required string Email { get; set; }

	[Range(18, 99)]
	public required int Age { get; set; }

	[MinLength(1)]
	public string? FirstName { get; set; }

	[MinLength(1)]
	public string? LastName { get; set; }
}
