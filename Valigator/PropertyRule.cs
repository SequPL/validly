using System.Collections.Immutable;
using Valigator.Validators;

namespace Valigator;

/// <summary>
/// PropertyRule is a class that defines a rule (or a set of rules) for validating a single property
/// </summary>
/// <typeparam name="TProperty"></typeparam>
public class PropertyRule<TProperty>
{
    public delegate IEnumerable<ValidationMessage> CustomValidator();
    public delegate IEnumerable<ValidationMessage> CustomContextValidator(
        ValidationContext validationContext
    );

    private readonly string _name;
    private readonly List<Validator> _validators;
    private readonly List<ContextValidator> _contextValidators;

    // public delegate void AddErrorDelegate(ValidationMessage message);
    // private readonly List<ValidationMessage> _errors = new(1);
    // public TProperty? PropertyValue { get; private set; }

    public PropertyRule(
        string name,
        List<Validator> validators,
        List<ContextValidator> contextValidators
    )
    {
        _name = name;
        _validators = validators;
        _contextValidators = contextValidators;
    }

    // public PropertyRule<TProperty> AndAlso(Action<AddErrorDelegate> validator)
    // {
    // 	validator(error => _errors.Add(error));
    // 	return this;
    // }
    //
    // public PropertyRule<TProperty> AndAlso(Func<IEnumerable<ValidationMessage>> validator)
    // {
    // 	_errors.AddRange(validator());
    // 	return this;
    // }
    //
    // public PropertyRule<TProperty> AndAlso(IEnumerable<ValidationMessage> erroMessages)
    // {
    // 	_errors.AddRange(erroMessages);
    // 	return this;
    // }

    /// <summary>
    /// Use the configured <see cref="PropertyRule{TProperty}" /> to validate the value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public PropertyValidationResult Validate(TProperty? value, ValidationContext? context)
    {
        var errors = EvaluateValidators(value, context);

        return new PropertyValidationResult
        {
            PropertyName = _name,
            Messages = errors.ToImmutableArray(),
        };
    }

    /// <summary>
    /// Use the configured <see cref="PropertyRule{TProperty}" /> along with the provided custom validator to validate the value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="context"></param>
    /// <param name="customValidator"></param>
    /// <returns></returns>
    public PropertyValidationResult Validate(
        TProperty? value,
        ValidationContext? context,
        CustomValidator customValidator
    )
    {
        var errors = EvaluateValidators(value, context);
        errors.AddRange(customValidator());

        return new PropertyValidationResult
        {
            PropertyName = _name,
            Messages = errors.ToImmutableArray(),
        };
    }

    /// <summary>
    /// Use the configured <see cref="PropertyRule{TProperty}" /> along with the provided custom validator to validate the value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="context"></param>
    /// <param name="customValidator"></param>
    /// <returns></returns>
    public PropertyValidationResult Validate(
        TProperty? value,
        ValidationContext context,
        CustomContextValidator customValidator
    )
    {
        var errors = EvaluateValidators(value, context);
        errors.AddRange(customValidator(context));

        return new PropertyValidationResult
        {
            PropertyName = _name,
            Messages = errors.ToImmutableArray(),
        };
    }

    private List<ValidationMessage> EvaluateValidators(TProperty? value, ValidationContext? context)
    {
        var errors = new List<ValidationMessage>(1);

        // Execute Validators
        foreach (Validator validator in _validators)
        {
            errors.AddRange(validator.IsValid(value));
        }

        // Execute ContextValidators
        foreach (ContextValidator validator in _contextValidators)
        {
            errors.AddRange(
                validator.IsValid(
                    value,
                    context
                        ?? throw new ArgumentNullException(
                            nameof(context),
                            "ValidationContext is required for ContextValidators"
                        )
                )
            );
        }

        return errors;
    }
}
