namespace Reflection.Assembly;

public class IntAscendingSortingProvider : ISortingProvider<int>
{
    public Sorting Sorting => Sorting.Ascending;


    public IReadOnlyCollection<int> Sort(IReadOnlyCollection<int> items)
    {
        return items.OrderBy(i => i).ToList();
    }
}