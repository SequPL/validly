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

// public partial class ValigatorCreateUserRequest
// 	: global::Valigator.IValidatable,
// 		global::Valigator.Validators.IInternalValidationInvoker
// {
// 	/// <inheritdoc />
// 	ValueTask<global::Valigator.ValidationResult> global::Valigator.Validators.IInternalValidationInvoker.Validate(
// 		global::Valigator.ValidationContext context,
// 		global::System.IServiceProvider? serviceProvider
// 	)
// 	{
// 		var result = new global::Valigator.ExtendableValidationResult(6);
// 		// Validate Username
// 		context.SetProperty("Username");
// 		result.AddPropertyResult(
// 			global::Valigator
// 				.PropertyValidationResult.Create(
// 					"Username",
// 					// ValigatorCreateUserRequestRules
// 					// 	.UsernameRule.Item1.IsValid(Username)
// 					// 	.Concat(ValigatorCreateUserRequestRules.UsernameRule.Item2.IsValid(Username))
// 					ValigatorCreateUserRequestRules.UsernameRule.Item2.IsValid(Username)
// 				// ConcatenatedEnumerable.From(
// 				// 	[ValigatorCreateUserRequestRules.UsernameRule.Item1.IsValid(Username)],
// 				// 	ValigatorCreateUserRequestRules.UsernameRule.Item2.IsValid(Username)
// 				// )
// 				)
// 				.AddValidationMessage(ValigatorCreateUserRequestRules.UsernameRule.Item1.IsValid(Username))
// 		);
//
// 		// Validate Password
// 		context.SetProperty("Password");
// 		result.AddPropertyResult(
// 			global::Valigator
// 				.PropertyValidationResult.Create(
// 					"Password",
// 					// ValigatorCreateUserRequestRules
// 					// 	.PasswordRule.Item1.IsValid(Password)
// 					// 	.Concat(ValigatorCreateUserRequestRules.PasswordRule.Item2.IsValid(Password))
//
// 					// TODO: Pokud by bylo více validátorů
// 					// ConcatenatedEnumerable.From(
// 					// 	ValigatorCreateUserRequestRules.PasswordRule.Item1.IsValid(Password),
// 					// 	ValigatorCreateUserRequestRules.PasswordRule.Item2.IsValid(Password)
// 					// )
// 					ValigatorCreateUserRequestRules.PasswordRule.Item2.IsValid(Password)
// 				)
// 				.AddValidationMessage(ValigatorCreateUserRequestRules.PasswordRule.Item1.IsValid(Password))
// 		);
//
// 		// Validate Email
// 		context.SetProperty("Email");
// 		result.AddPropertyResult(
// 			global::Valigator
// 				.PropertyValidationResult.Create(
// 					"Email",
// 					// ValigatorCreateUserRequestRules
// 					// 	.EmailRule.Item1.IsValid(Email)
// 					// 	.Concat(ValigatorCreateUserRequestRules.EmailRule.Item2.IsValid(Email))
// 					ValigatorCreateUserRequestRules.EmailRule.Item2.IsValid(Email)
// 				// ConcatenatedEnumerable.From(ValigatorCreateUserRequestRules.EmailRule.Item2.IsValid(Email))
// 				)
// 				.AddValidationMessage(ValigatorCreateUserRequestRules.EmailRule.Item1.IsValid(Email))
// 		);
//
// 		// Validate Age
// 		context.SetProperty("Age");
// 		result.AddPropertyResult(
// 			global::Valigator.PropertyValidationResult.Create(
// 				"Age",
// 				ValigatorCreateUserRequestRules.AgeRule.Item1.IsValid(Age)
// 			)
// 		);
//
// 		// Validate FirstName
// 		context.SetProperty("FirstName");
// 		result.AddPropertyResult(
// 			global::Valigator.PropertyValidationResult.Create(
// 				"FirstName",
// 				ValigatorCreateUserRequestRules.FirstNameRule.Item1.IsValid(FirstName)
// 			)
// 		);
//
// 		// Validate LastName
// 		context.SetProperty("LastName");
// 		result.AddPropertyResult(
// 			global::Valigator.PropertyValidationResult.Create(
// 				"LastName",
// 				ValigatorCreateUserRequestRules.LastNameRule.Item1.IsValid(LastName)
// 			)
// 		);
//
// 		return ValueTask.FromResult<global::Valigator.ValidationResult>(result);
// 	}
//
// 	/// <inheritdoc />
// 	ValueTask<global::Valigator.ValidationResult> global::Valigator.IValidatable.Validate(
// 		IServiceProvider serviceProvider
// 	)
// 	{
// 		using var validationContext = global::Valigator.ValidationContext.Create(this);
// 		return ((global::Valigator.Validators.IInternalValidationInvoker)this).Validate(
// 			validationContext,
// 			serviceProvider
// 		);
// 	}
//
// 	/// <summary>Validate the object.</summary>
// 	public virtual global::Valigator.ValidationResult Validate()
// 	{
// 		using var validationContext = global::Valigator.ValidationContext.Create(this);
// 		return ((global::Valigator.Validators.IInternalValidationInvoker)this).Validate(validationContext, null).Result;
// 	}
// }
//
// file static class ValigatorCreateUserRequestRules
// {
// 	internal static readonly ValueTuple<
// 		global::Valigator.Extensions.Validators.Common.RequiredAttribute,
// 		global::Valigator.Extensions.Validators.Strings.LengthBetweenAttribute
// 	> UsernameRule = (
// 		new global::Valigator.Extensions.Validators.Common.RequiredAttribute(false),
// 		new global::Valigator.Extensions.Validators.Strings.LengthBetweenAttribute(5, 20)
// 	);
//
// 	internal static readonly ValueTuple<
// 		global::Valigator.Extensions.Validators.Common.RequiredAttribute,
// 		global::Valigator.Extensions.Validators.Strings.MinLengthAttribute
// 	> PasswordRule = (
// 		new global::Valigator.Extensions.Validators.Common.RequiredAttribute(false),
// 		new global::Valigator.Extensions.Validators.Strings.MinLengthAttribute(12)
// 	);
//
// 	internal static readonly ValueTuple<
// 		global::Valigator.Extensions.Validators.Common.RequiredAttribute,
// 		global::Valigator.Extensions.Validators.Strings.EmailAddressAttribute
// 	> EmailRule = (
// 		new global::Valigator.Extensions.Validators.Common.RequiredAttribute(false),
// 		new global::Valigator.Extensions.Validators.Strings.EmailAddressAttribute()
// 	);
//
// 	internal static readonly ValueTuple<global::Valigator.Extensions.Validators.Numbers.BetweenAttribute> AgeRule = (
// 		ValueTuple.Create(new global::Valigator.Extensions.Validators.Numbers.BetweenAttribute(18, 99))
// 	);
//
// 	internal static readonly ValueTuple<global::Valigator.Extensions.Validators.Strings.NotEmptyAttribute> FirstNameRule =
// 		(ValueTuple.Create(new global::Valigator.Extensions.Validators.Strings.NotEmptyAttribute()));
//
// 	internal static readonly ValueTuple<global::Valigator.Extensions.Validators.Strings.NotEmptyAttribute> LastNameRule =
// 		(ValueTuple.Create(new global::Valigator.Extensions.Validators.Strings.NotEmptyAttribute()));
// }
