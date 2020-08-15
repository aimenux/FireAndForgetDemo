using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FireAndForgetDemo.Examples;
using FireAndForgetDemo.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FireAndForgetDemo
{
    public static class Program
    {
        public static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.AddTransient<IExample, Example1>();
            services.AddTransient<IExample, Example2>();
            services.AddSingleton<ITaskHelper, TaskHelper>();

            services.AddLogging(builder =>
            {
                builder.AddConsole(options =>
                {
                    options.DisableColors = true;
                    options.TimestampFormat = "[HH:mm:ss:fff] ";
                });
                builder.AddNonGenericLogger();
            });
            
            services.Configure<Settings>(configuration.GetSection(nameof(Settings)));

            var serviceProvider = services.BuildServiceProvider();
            var examples = serviceProvider.GetServices<IExample>().ToList();

            var sw = new Stopwatch();

            foreach (var example in examples)
            {
                var name = example.GetType().Name;
                var description = example.Description;
                ConsoleColor.Green.WriteLine($"{name} -> {description}");
                sw.Start();
                await example.RunAsync();
                sw.Stop();
                ConsoleColor.Green.WriteLine($"{name} -> Elapsed time '{sw.ElapsedMilliseconds}' ms\n");
            }

            Console.WriteLine("Press any key to exit !\n");
            Console.ReadKey();
        }
    }
}
