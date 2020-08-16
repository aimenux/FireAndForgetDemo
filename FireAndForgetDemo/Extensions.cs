using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FireAndForgetDemo
{
    public static class Extensions
    {
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
    }
}