using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetAssessmentLevel4;

public static class CancelTasksDemo
{
    static readonly CancellationTokenSource s_cts = new ();

    public static async Task Start()
    {
        Console.WriteLine("Application started.");
        Console.WriteLine("Press the ENTER key to cancel...\n");

        await Task.Delay(3000);

        var cancelTask = Task.Run(() =>
        {
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Console.WriteLine("Press the ENTER key to cancel...");
            }

            Console.WriteLine("\nENTER key pressed: cancelling .\n");
            s_cts.Cancel();
        });

        var countAsyncTasks = CountAsync();

        await Task.WhenAny(new[] { cancelTask, countAsyncTasks });

        Console.WriteLine("Application ending.");
    }

    public static async Task CountAsync()
    {
        var x = 0;

        await Task.Run(async () =>
        {
            while (true)
            {
                Console.WriteLine(x);
                x++;
                
                await Task.Delay(500);
            }
        });
    }
}