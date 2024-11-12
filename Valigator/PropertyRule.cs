// using System.Collections.Immutable;
// using Valigator.Validators;
//
// namespace Valigator;
//
// /// <summary>
// /// PropertyRule is a class that defines a rule (or a set of rules) for validating a single property
// /// </summary>
// /// <typeparam name="TProperty"></typeparam>
// public class PropertyRule<TProperty>
// {
// 	/// <summary>
// 	/// Delegate for custom validator
// 	/// </summary>
// 	public delegate IEnumerable<ValidationMessage> CustomValidator();
//
// 	/// <summary>
// 	/// Delegate for custom validator with context
// 	/// </summary>
// 	public delegate IEnumerable<ValidationMessage> CustomContextValidator(ValidationContext validationContext);
//
// 	/// <summary>
// 	/// Delegate for custom async validator
// 	/// </summary>
// 	public delegate IAsyncEnumerable<ValidationMessage> CustomAsyncValidator();
//
// 	/// <summary>
// 	/// Delegate for custom async validator with context
// 	/// </summary>
// 	public delegate IAsyncEnumerable<ValidationMessage> CustomAsyncContextValidator(
// 		ValidationContext validationContext
// 	);
//
// 	private readonly string _name;
// 	private readonly List<IValidator> _validators;
// 	private readonly List<IContextValidator> _contextValidators;
// 	private readonly List<IAsyncValidator> _asyncValidators;
// 	private readonly List<IAsyncContextValidator> _asyncContextValidators;
//
// 	/// <param name="name"></param>
// 	/// <param name="validators"></param>
// 	/// <param name="contextValidators"></param>
// 	/// <param name="asyncValidators"></param>
// 	/// <param name="asyncContextValidators"></param>
// 	public PropertyRule(
// 		string name,
// 		List<IValidator> validators,
// 		List<IContextValidator> contextValidators,
// 		List<IAsyncValidator> asyncValidators,
// 		List<IAsyncContextValidator> asyncContextValidators
// 	)
// 	{
// 		_name = name;
// 		_validators = validators;
// 		_contextValidators = contextValidators;
// 		_asyncValidators = asyncValidators;
// 		_asyncContextValidators = asyncContextValidators;
// 	}
//
// 	/// <summary>
// 	/// Use the configured <see cref="PropertyRule{TProperty}" /> to validate the value.
// 	/// </summary>
// 	/// <param name="value"></param>
// 	/// <param name="context"></param>
// 	/// <returns></returns>
// 	public PropertyValidationResult Validate(TProperty? value, ValidationContext? context)
// 	{
// 		return new PropertyValidationResult(_name, EvaluateValidators(value, context));
// 	}
//
// 	/// <summary>
// 	/// Use the configured <see cref="PropertyRule{TProperty}" /> along with the provided custom validator to validate the value
// 	/// </summary>
// 	/// <param name="value"></param>
// 	/// <param name="context"></param>
// 	/// <param name="customValidator"></param>
// 	/// <returns></returns>
// 	public PropertyValidationResult Validate(
// 		TProperty? value,
// 		ValidationContext? context,
// 		CustomValidator customValidator
// 	)
// 	{
// 		return new PropertyValidationResult(_name, EvaluateValidators(value, context).Concat(customValidator()));
// 	}
//
// 	/// <summary>
// 	/// Use the configured <see cref="PropertyRule{TProperty}" /> along with the provided custom validator to validate the value
// 	/// </summary>
// 	/// <param name="value"></param>
// 	/// <param name="context"></param>
// 	/// <param name="customValidator"></param>
// 	/// <returns></returns>
// 	public PropertyValidationResult Validate(
// 		TProperty? value,
// 		ValidationContext context,
// 		CustomContextValidator customValidator
// 	)
// 	{
// 		return new PropertyValidationResult(_name, EvaluateValidators(value, context).Concat(customValidator(context)));
// 	}
//
// 	private IEnumerable<ValidationMessage> EvaluateValidators(TProperty? value, ValidationContext? context)
// 	{
// 		// Execute Validators
// 		foreach (IValidator validator in _validators)
// 		{
// 			foreach (ValidationMessage message in validator.IsValid(value))
// 			{
// 				yield return message;
// 			}
// 		}
//
// 		// Execute ContextValidators
// 		foreach (IContextValidator validator in _contextValidators)
// 		{
// 			var messages = validator.IsValid(
// 				value,
// 				context
// 					?? throw new ArgumentNullException(
// 						nameof(context),
// 						"ValidationContext is required for ContextValidators"
// 					)
// 			);
//
// 			foreach (ValidationMessage message in messages)
// 			{
// 				yield return message;
// 			}
// 		}
// 	}
//
// 	private async IAsyncEnumerable<ValidationMessage> EvaluateAsyncValidators(
// 		TProperty? value,
// 		ValidationContext? context
// 	)
// 	{
// 		// Execute Validators
// 		foreach (IAsyncValidator validator in _asyncValidators)
// 		{
// 			await foreach (ValidationMessage message in validator.IsValid(value))
// 			{
// 				yield return message;
// 			}
// 		}
//
// 		// Execute ContextValidators
// 		foreach (IAsyncContextValidator validator in _asyncContextValidators)
// 		{
// 			var messages = validator.IsValid(
// 				value,
// 				context
// 					?? throw new ArgumentNullException(
// 						nameof(context),
// 						"ValidationContext is required for ContextValidators"
// 					)
// 			);
//
// 			await foreach (ValidationMessage message in messages)
// 			{
// 				yield return message;
// 			}
// 		}
// 	}
// }
