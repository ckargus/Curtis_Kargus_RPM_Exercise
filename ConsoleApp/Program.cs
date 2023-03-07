namespace ConsoleApp
{
    using FluentScheduler;
    using Microsoft.Extensions.DependencyInjection;

    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press the enter key to stop background process");
            var services = new ServiceCollection();
            Startup.ConfigureServices(services);

            var executor = services.AddSingleton<Executor, Executor>()
                .BuildServiceProvider()
                .GetService<Executor>();

            if(executor != null)
            {
                executor.Execute();
            }
            else
            {
                Console.WriteLine("Unable to find Executor Service");
            }
            Console.ReadLine();
            JobManager.StopAndBlock();
            
        }
    }
}