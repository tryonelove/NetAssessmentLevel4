namespace NetAssessmentLevel4;

public static class AutoResetEventDemo
{
    private static readonly AutoResetEvent AutoResetEvent = new (false);


    public static void Start()
    {
        Console.WriteLine("Concert");
        Console.WriteLine("Press enter to allow visitors to enter");
        Console.ReadLine();
            
        for (var i = 1; i < 4; i++)
        {
            var t = new Thread(SolvePatientProblem)
            {
                Name = "Visitor " + i
            };

            t.Start();
        }
        
        Thread.Sleep(250);
        Console.WriteLine("QA Session has started! Come talk to me!");

        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine("Allow visitor to enter");
            AutoResetEvent.Set();
            Console.ReadLine(); 
        }
    }

    private static void SolvePatientProblem()
    {
        var name = Thread.CurrentThread.Name;

        Console.WriteLine("{0} is here.", name);
        AutoResetEvent.WaitOne();
        
        Console.WriteLine("{0} is gone. Next one.", name);
    }
}