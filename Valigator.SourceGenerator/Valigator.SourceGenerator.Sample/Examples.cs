// using System.Collections.Generic;
// using Valigator.Extensions.Validators.Common;
// using Valigator.Extensions.Validators.Strings;
// using Valigator.Validators;
//
// namespace Valigator.SourceGenerator.Sample;
//
// //
// // public static class ValidationsErrors
// // {
// // 	public static readonly AppString TestError =
// // 		new("ValidationsErrors.TestError", "This is a test error. Value '{0}' is not valid.");
// // }
//
// [Validatable]
// public partial class FooArgs
// {
// 	[Required]
// 	[MinLength(5)]
// 	public required string RequiredString { get; set; }
//
// 	[EmailAddress]
// 	[CustomValidation]
// 	public required string SomeEmail { get; set; }
//
// 	// private void Validation(ValidationContext context)
// 	// {
// 	//  RequiredStringRule
// 	//   .Use(new PropertyRule<>.CustomValidator(RequiredString, SomeEmail))
// 	//   .AndAlso(error =>
// 	//   {
// 	//    if (string.IsNullOrWhiteSpace(RequiredString))
// 	//    {
// 	//     error(new ResultError(new AppString("xxx")));
// 	//    }
// 	//   });
// 	//
// 	//  EmailRule.AndAlso(CustomEmailValidation(context));
// 	// }
//
// 	// private IEnumerable<ValidationMessage> CustomEmailValidation(ValidationContext context)
// 	// {
// 	//  throw new NotImplementedException();
// 	// }
// 	//
// 	// private IEnumerable<ResultError> CustomEmailValidation(ValidationContext c)
// 	// {
// 	// 	if (SomeEmail.Contains("localhost"))
// 	// 	{
// 	// 		yield return new ResultError(new AppString("xxx"));
// 	// 	}
// 	// }
//
// 	// protected partial IEnumerable<ValidationMessage> ValidateRequiredString()
// 	// {
// 	// 	if (string.IsNullOrWhiteSpace(RequiredString))
// 	// 	{
// 	// 		yield return new ValidationMessage();
// 	// 	}
// 	// }
// 	public IEnumerable<ValidationMessage> ValidateSomeEMail()
// 	{
// 		if (SomeEmail.Contains("@"))
// 		{
// 			yield return new ValidationMessage("Fuck you, it is not email", SomeEmail);
// 		}
// 	}
// }
//
// // // Toto bude generovat source generator
// // file interface IFooArgsCustomValidator
// // {
// // 	IEnumerable<ValidationMessage> ValidateSomeEMail();
// // }
// //
// // public partial class FooArgs : IFooArgsCustomValidator
// // {
// // 	private static PropertyRule<string> RequiredStringRule { get; } =
// // 		new PropertyRuleBuilder<string>("RequiredString")
// // 			.Use(new RequiredValidator())
// // 			.Use(new MinLengthValidator(5))
// // 			.Build();
// //
// // 	private static PropertyRule<string> EmailRule { get; } =
// // 		new PropertyRuleBuilder<string>("SomeEmail").Use(new EmailAddressValidator()).Build();
// //
// // 	// public ValidationResult Validate()
// // 	// {
// // 	// 	var customValidator = (IFooArgsCustomValidator)this;
// // 	//
// // 	// 	return new ValidationResult(
// // 	// 		// [],
// // 	// 		RequiredStringRule.Validate(RequiredString, null, customValidator.ValidateRequiredString),
// // 	// 		EmailRule.Validate(SomeEmail, null)
// // 	// 	);
// // 	// }
// //
// // 	public ValidationResult Validate(ValidationContext context)
// // 	{
// // 		var customValidator = (IFooArgsCustomValidator)this;
// //
// // 		return new ValidationResult(
// // 			[],
// // 			RequiredStringRule.Validate(RequiredString, context, customValidator.ValidateRequiredString),
// // 			EmailRule.Validate(SomeEmail, context)
// // 		);
// // 	}
// // }
// //
// //
// // class Program
// // {
// // 	void Foo(FooArgs args)
// // 	{
// // 		var result = args.Validate(ValidationContext);
// //
// // 		if (!result.Valid)
// // 		{
// // 			result.Properties.
// // 			return;
// // 		}
// // 	}
// // }
