namespace Validly.SourceGenerator.Builders;

public class DependenciesTracker
{
	private readonly HashSet<string> _services = new();

	public bool HasDependencies { get; private set; }

	public IReadOnlyCollection<string> Services => _services;

	public void AddDependency(string dependency)
	{
		HasDependencies = true;

		if (
			dependency
			is Consts.ValidationContextName
				or Consts.ValidationContextQualifiedName
				or Consts.ValidationResultName
				or Consts.ValidationResultQualifiedName
				or Consts.ExtendableValidationResultName
				or Consts.ExtendableValidationResultQualifiedName
		)
		{
			return;
		}

		_services.Add(dependency);
	}
}
