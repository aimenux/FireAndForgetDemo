using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FireAndForgetDemo
{
    public static class Extensions
    {
        public static void FireAndForgetTask(this Task task, string taskName = null, [CallerMemberName] string callerName = null)
        {
            void OnFaulted(Task faultedTask)
            {
                var exception = faultedTask.Exception;
                var name = taskName ?? callerName;
                ConsoleColor.Red.WriteLine($"An error has occured on fire and forget task: {name} {exception}");
            }

            Task.Run(() => task).ContinueWith(OnFaulted, TaskContinuationOptions.OnlyOnFaulted);
        }

        public static void AddNonGenericLogger(this ILoggingBuilder loggingBuilder)
        {
            var services = loggingBuilder.Services;
            services.AddSingleton(serviceProvider =>
            {
                const string categoryName = nameof(Program);
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return loggerFactory.CreateLogger(categoryName);
            });
        }

        public static void WriteLine(this ConsoleColor color, object value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}
