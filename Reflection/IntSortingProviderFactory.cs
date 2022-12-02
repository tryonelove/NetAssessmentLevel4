using Reflection.Assembly;

namespace Reflection;

public class IntSortingProviderFactory
{
    private readonly IReadOnlyCollection<ISortingProvider<int>> _sortingProviders;


    private IntSortingProviderFactory()
    {
        var sortingProviderType = typeof(ISortingProvider<int>);

        var sortingProviderTypeInheritors = sortingProviderType
            .Assembly
            .ExportedTypes
            .Where(t => sortingProviderType.IsAssignableFrom(t))
            .ToList();

        _sortingProviders = sortingProviderTypeInheritors
            .Select(Activator.CreateInstance)
            .Cast<ISortingProvider<int>>()
            .ToList();
    }


    public ISortingProvider<int> GetProvider(Sorting sorting)
    {
        var provider = _sortingProviders.First(sp => sp.Sorting == sorting);

        return provider;
    }


    public static IntSortingProviderFactory Create()
    {
        return new IntSortingProviderFactory();
    }
}
