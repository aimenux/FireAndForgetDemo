using System.Threading.Tasks;
using FireAndForgetDemo.Helpers;
using Microsoft.Extensions.Logging;

namespace FireAndForgetDemo.Examples
{
    public abstract class AbstractExample : IExample
    {
        protected ITaskHelper TaskHelper; 
        protected ILogger Logger;

        protected AbstractExample(Settings settings, ITaskHelper taskHelper, ILogger logger)
        {
            Logger = logger;
            TaskHelper = taskHelper;
            AwaitableTaskName = $"{GetType().Name}.{settings.AwaitableTaskName}";
            AwaitableTaskDurationInSeconds = settings.AwaitableTaskDurationInSeconds;
            FireAndForgetTaskName = $"{GetType().Name}.{settings.FireAndForgetTaskName}";
            FireAndForgetTaskDurationInSeconds = settings.FireAndForgetTaskDurationInSeconds;
        }
        public string AwaitableTaskName { get; }
        public int AwaitableTaskDurationInSeconds { get; }
        public string FireAndForgetTaskName { get; }
        public int FireAndForgetTaskDurationInSeconds { get; }

        public abstract string Description { get; }
        public abstract Task RunAsync();
    }
}
