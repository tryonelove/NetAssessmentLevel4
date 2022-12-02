namespace Reflection;

public class Shop
{
    private readonly int _beerCount;
    

    public Shop()
    {
        _beerCount = 6;
    }


    [Over18]
    public void SellBeer(User user)
    {
        Console.WriteLine($"By the way, we have {_beerCount} beers.");
        
        Console.WriteLine(user.Age > 18 ? "Successfully sold a couple of beers" : "Uhh come back later");
    }
}