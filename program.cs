using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("---- .NET Thread and Env Test ----");

        Console.WriteLine($"Environment variable DOTNET_PROCESSOR_COUNT: {Environment.GetEnvironmentVariable("DOTNET_PROCESSOR_COUNT")}");
        Console.WriteLine($"Environment variable DOTNET_THREADPOOL_MINTHREADS: {Environment.GetEnvironmentVariable("DOTNET_THREADPOOL_MINTHREADS")}");
        
        // Get current processor count visible to the .NET runtime
        Console.WriteLine($"Environment.ProcessorCount: {Environment.ProcessorCount}");

        // Get current thread pool settings
        ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
        Console.WriteLine($"ThreadPool Min Threads: {workerThreads}, Completion Port Threads: {completionPortThreads}");

        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
        Console.WriteLine($"ThreadPool Max Threads: {maxWorkerThreads}, Completion Port Threads: {maxCompletionPortThreads}");

        Console.WriteLine("---- Done ----");
    }
}