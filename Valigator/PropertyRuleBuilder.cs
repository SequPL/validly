using Valigator.Validators;

namespace Valigator;

/// <summary>
/// Builder for creating a <see cref="PropertyRule{TProperty}" />
/// </summary>
/// <typeparam name="TProperty"></typeparam>
public class PropertyRuleBuilder<TProperty>
{
    private readonly string _propertyName;
    private readonly List<Validator> _validators = new();
    private readonly List<ContextValidator> _contextValidators = new();

    public PropertyRuleBuilder(string propertyName)
    {
        _propertyName = propertyName;
    }

    /// <summary>
    /// Use the provided <see cref="Validator" /> to validate the property
    /// </summary>
    /// <param name="validator"></param>
    /// <returns></returns>
    public PropertyRuleBuilder<TProperty> Use(Validator validator)
    {
        _validators.Add(validator);
        return this;
    }

    /// <summary>
    /// Use the provided <see cref="Validator" /> to validate the property
    /// </summary>
    /// <param name="validator"></param>
    /// <returns></returns>
    public PropertyRuleBuilder<TProperty> Use(ContextValidator validator)
    {
        _contextValidators.Add(validator);
        return this;
    }

    /// <summary>
    /// Build the final <see cref="PropertyRule{TProperty}" />
    /// </summary>
    /// <returns></returns>
    public PropertyRule<TProperty> Build()
    {
        return new PropertyRule<TProperty>(_propertyName, _validators, _contextValidators);
    }
}
