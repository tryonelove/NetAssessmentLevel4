namespace NetAssessmentLevel4;

public static class ManualResetEventDemo
{
    private static readonly ManualResetEvent Mre = new (false);


    public static void Start()
    {
        for (var i = 0; i <= 2; i++)
        {
            var t = new Thread(MakePizza)
            {
                Name = $"Pizza {i}"
            };
            t.Start();
        }

        Thread.Sleep(500);
        Console.WriteLine("Pizza is done. Press enter to get all of them at once.");
        Console.ReadLine();

        Mre.Set();

        Thread.Sleep(500);

        Console.WriteLine("Can I have a bunch of pizza slices?");
        
        for (var i = 0; i <= 3; i++)
        {
            var t = new Thread(MakePizza)
            {
                Name = $"Pizza-slice {i}"
            };

            t.Start();
        }

        Thread.Sleep(500);
        
        Console.WriteLine("Can I have two number 9s?");
        Mre.Reset();

        var firstNumberNine = new Thread(MakePizza)
        {
            Name = "Number9 - 1"
        };
        firstNumberNine.Start();

        var secondNumberNine = new Thread(MakePizza)
        {
            Name = "Number9 - 2"
        };
        secondNumberNine.Start();
        
        Thread.Sleep(500);
        Console.WriteLine("Press enter to get your number 9s and a laaarge soda");
        Console.ReadLine();

        Mre.Set();
    }


    private static void MakePizza()
    {
        var name = Thread.CurrentThread.Name!;

        Console.WriteLine($"{name} is in progress. Wait a bit.");

        Mre.WaitOne();

        Console.WriteLine(name + " given out.");
    }
}