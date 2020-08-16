using System;
using System.Diagnostics;
using System.IO;
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
            services.AddTransient<IExample, Example3>();
            services.AddSingleton<ITaskHelper, TaskHelper>();
            services.AddSingleton<IStubHelper, StubHelper>();

            services.AddLogging(builder =>
            {
                builder.AddConsole(options =>
                {
                    options.DisableColors = false;
                    options.TimestampFormat = "[HH:mm:ss:fff] ";
                });
                builder.AddNonGenericLogger();
                builder.SetMinimumLevel(LogLevel.Trace);
            });
            
            services.Configure<Settings>(configuration.GetSection(nameof(Settings)));

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger>();
            var examples = serviceProvider.GetServices<IExample>();

            var sw = new Stopwatch();

            foreach (var example in examples)
            {
                var name = example.GetType().Name;
                var description = example.Description;
                logger.LogInformation($"{name} -> {description}");
                sw.Start();
                await example.RunAsync();
                sw.Stop();
                logger.LogInformation($"{name} -> Elapsed time '{sw.ElapsedMilliseconds}' ms");
            }

            Console.WriteLine("Press any key to exit !\n");
            Console.ReadKey();
        }
    }
}
