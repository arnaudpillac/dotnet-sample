using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Configuration
        int threadCount = Environment.ProcessorCount;
        int memoryPerThreadMb = 100;
        int iterations = 1_000_000;

        // Parse args
        foreach (var arg in args)
        {
            if (arg.StartsWith("--threads="))
                int.TryParse(arg.Substring("--threads=".Length), out threadCount);
            if (arg.StartsWith("--mb="))
                int.TryParse(arg.Substring("--mb=".Length), out memoryPerThreadMb);
            if (arg.StartsWith("--iterations="))
                int.TryParse(arg.Substring("--iterations=".Length), out iterations);
        }

        Console.WriteLine("---- .NET Infinite Load Test ----");
        Console.WriteLine($"Thread count: {threadCount}");
        Console.WriteLine($"Memory per thread: {memoryPerThreadMb} MB");
        Console.WriteLine($"Iterations per task loop: {iterations}");

        // Start infinite load
        while (true)
        {
            var stopwatch = Stopwatch.StartNew();

            Parallel.For(0, threadCount, new ParallelOptions { MaxDegreeOfParallelism = threadCount }, i =>
            {
                byte[] buffer = new byte[memoryPerThreadMb * 1024 * 1024];
                long sum = 0;
                for (int j = 0; j < iterations; j++)
                {
                    sum += j ^ i;
                    if (j % 100_000 == 0)
                        buffer[j % buffer.Length] = (byte)(j % 256);
                }
            });

            stopwatch.Stop();
            Console.WriteLine($"[Loop] Load iteration completed in {stopwatch.Elapsed.TotalSeconds:F2} sec");
            LogSystemStats();
        }
    }

    static void LogSystemStats()
    {
        var proc = Process.GetCurrentProcess();
        Console.WriteLine("--- Stats ---");
        Console.WriteLine($"Env.ProcessorCount: {Environment.ProcessorCount}");
        Console.WriteLine($"WorkingSet: {(proc.WorkingSet64 / 1024.0 / 1024.0):F2} MB");
        Console.WriteLine($"GC TotalMemory: {(GC.GetTotalMemory(false) / 1024.0 / 1024.0):F2} MB");
        Console.WriteLine("--------------");
    }
}
