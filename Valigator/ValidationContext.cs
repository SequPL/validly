using Microsoft.Extensions.DependencyInjection;

namespace Valigator;

/// <summary>
/// Validation context for use with <see cref="Validators.ContextValidator" />.
/// It holds the <see cref="IServiceProvider" /> and provides a way to get services from it.
/// </summary>
public class ValidationContext : IServiceProvider
{
    private readonly Lazy<IServiceScope> _serviceScope;

    /// <summary></summary>
    /// <param name="serviceProvider"></param>
    public ValidationContext(IServiceProvider serviceProvider)
    {
        _serviceScope = new Lazy<IServiceScope>(serviceProvider.CreateScope);
    }

    public object? GetService(Type serviceType)
    {
        return _serviceScope.Value.ServiceProvider.GetService(serviceType);
    }
}
