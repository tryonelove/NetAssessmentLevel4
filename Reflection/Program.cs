using System.Reflection;
using Reflection;
using Reflection.Assembly;

var values = new[] { 1, 2, 3, 4 };

var sortingFactory = IntSortingProviderFactory.Create();

var sortedByAscValues = sortingFactory
    .GetProvider(Sorting.Ascending)
    .Sort(values)
    .ToList();
Console.WriteLine(string.Join(" ", sortedByAscValues));

var sortedByDescValues = sortingFactory
    .GetProvider(Sorting.Descending)
    .Sort(values)
    .ToList();
Console.WriteLine(string.Join(" ", sortedByDescValues));

var notAllowedAge = new User("Ilya", 12);
var allowedAge = new User("ILYA", 21);

var attributeType = typeof(Over18Attribute);
var shopType = typeof(Shop);

var method = shopType.GetMethods()
    .First(mi => mi.GetCustomAttribute(attributeType, false) != null);

method.Invoke(Activator.CreateInstance(shopType), new object?[] { notAllowedAge });
method.Invoke(Activator.CreateInstance(shopType), new object?[] { allowedAge });
