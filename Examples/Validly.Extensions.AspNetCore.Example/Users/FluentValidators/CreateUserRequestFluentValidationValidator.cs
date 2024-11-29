using FluentValidation;
using Validly.Extensions.AspNetCore.Example.Users.Dtos;

public class CreateUserRequestFluentValidationValidator : AbstractValidator<CreateUserRequest>
{
	public CreateUserRequestFluentValidationValidator()
	{
		RuleFor(x => x.Username).NotEmpty().Length(5, 20);
		RuleFor(x => x.Password).NotEmpty().MinimumLength(12);
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
		RuleFor(x => x.Age).InclusiveBetween(18, 99);
		RuleFor(x => x.FirstName).NotEmpty().When(x => x.LastName is not null);
		RuleFor(x => x.LastName).NotEmpty().When(x => x.LastName is not null);
	}
}
