namespace Reflection.Assembly;

public class IntDescendingSortingProvider : ISortingProvider<int>
{
    public Sorting Sorting => Sorting.Descending;


    public IReadOnlyCollection<int> Sort(IReadOnlyCollection<int> items)
    {
        return items.OrderByDescending(i => i).ToList();
    }
}