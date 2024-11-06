namespace Valigator.SourceGenerator.Sample;

// [Validatable]
public partial class CreateUserRequest
{
	// [Required]
	// [MinLength(5)]
	public required string Name { get; set; }

	// [EmailAddress]
	// [CustomValidation]
	public required string Email { get; set; }
}
