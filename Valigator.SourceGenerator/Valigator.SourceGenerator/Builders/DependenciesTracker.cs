namespace Valigator.SourceGenerator.Builders;

public class DependenciesTracker
{
	private HashSet<string> _dependencies = new();

	public IReadOnlyCollection<string> Dependencies => _dependencies;

	public void AddDependency(string dependency)
	{
		if (dependency == Consts.ValidationContextName)
		{
			return;
		}

		_dependencies.Add(dependency);
	}
}
