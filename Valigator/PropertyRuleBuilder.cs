// using Valigator.Validators;
//
// namespace Valigator;
//
// /// <summary>
// /// Builder for creating a <see cref="PropertyRule{TProperty}" />
// /// </summary>
// /// <typeparam name="TProperty"></typeparam>
// public class PropertyRuleBuilder<TProperty>
// {
// 	private readonly string _propertyName;
// 	private readonly List<IValidator> _validators = new(1);
// 	private readonly List<IContextValidator> _contextValidators = new(0);
// 	private readonly List<IAsyncValidator> _asyncValidators = new(0);
// 	private readonly List<IAsyncContextValidator> _asyncContextValidators = new(0);
//
// 	/// <param name="propertyName"></param>
// 	public PropertyRuleBuilder(string propertyName)
// 	{
// 		_propertyName = propertyName;
// 	}
//
// 	/// <summary>
// 	/// Use the provided <see cref="IValidator" /> to validate the property
// 	/// </summary>
// 	/// <param name="validator"></param>
// 	/// <returns></returns>
// 	public PropertyRuleBuilder<TProperty> Use(IValidator validator)
// 	{
// 		_validators.Add(validator);
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Use the provided <see cref="IContextValidator" /> to validate the property
// 	/// </summary>
// 	/// <param name="validator"></param>
// 	/// <returns></returns>
// 	public PropertyRuleBuilder<TProperty> Use(IContextValidator validator)
// 	{
// 		_contextValidators.Add(validator);
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Build the final <see cref="PropertyRule{TProperty}" />
// 	/// </summary>
// 	/// <returns></returns>
// 	public PropertyRule<TProperty> Build()
// 	{
// 		return new PropertyRule<TProperty>(
// 			_propertyName,
// 			_validators,
// 			_contextValidators,
// 			_asyncValidators,
// 			_asyncContextValidators
// 		);
// 	}
// }
