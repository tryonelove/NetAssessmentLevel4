namespace Reflection.Assembly;

public interface ISortingProvider<T>
{
    public Sorting Sorting { get; }

    public IReadOnlyCollection<T> Sort(IReadOnlyCollection<T> items);
}