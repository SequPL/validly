using Valigator.Utils;
using Valigator.Validators;

namespace Valigator.SourceGenerator;

internal static class Consts
{
	public const string CustomValidationAttribute = $"Valigator.Validators.{nameof(CustomValidationAttribute)}";
	public const string ValidatableAttributeQualifiedName = $"Valigator.{nameof(ValidatableAttribute)}";
	public const string ValidatorAttributeQualifiedName = $"Valigator.Validators.{nameof(ValidatorAttribute)}";

	public const string InternalValidationInvokerGlobalRef =
		$"global::Valigator.Validators.{nameof(IInternalValidationInvoker)}";
	public const string IValidatableGlobalRef = $"global::Valigator.{nameof(IValidatable)}";
	public const string ValidationResultGlobalRef = $"global::Valigator.{nameof(ValidationResult)}";
	public const string PropertyValidationResultGlobalRef = $"global::Valigator.{nameof(PropertyValidationResult)}";
	public const string ValidationContextGlobalRef = $"global::Valigator.{nameof(ValidationContext)}";
	public const string ExtendableValidationResultGlobalRef = $"global::Valigator.{nameof(ExtendableValidationResult)}";
	public const string ValidationResultHelperGlobalRef = $"global::Valigator.Utils.{nameof(ValidationResultHelper)}";
	public const string ServiceProviderGlobalRef = $"global::System.{nameof(IServiceProvider)}";

	public const string ValidationResultName = "ValidationResult";
	public const string CustomValidationMethodPrefix = "Validate";
	public const string ValidationContextName = "ValidationContext";
	public const string ValidationMessageName = "ValidationMessage";
	public const string IsValidMethodName = "IsValid";
	public const string BeforeValidateMethodName = "BeforeValidate";
	public const string AfterValidateMethodName = "AfterValidate";
}
