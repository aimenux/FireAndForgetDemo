using System;
using System.Threading.Tasks;
using FireAndForgetDemo.Exceptions;
using FireAndForgetDemo.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FireAndForgetDemo.Examples
{
    public class Example2 : AbstractExample
    {
        public Example2(IOptions<Settings> options, ITaskHelper taskHelper, ILogger logger) : base(options.Value, taskHelper, logger)
        {
            Logger = logger;
        }

        public override string Description { get; } = "Run unsuccessful fire/forget task";

        public override Task RunAsync()
        {
            var awaitableTask = Task.Run(() =>
            {
                Logger.LogInformation($"Running '{AwaitableTaskName}' during '{AwaitableTaskDurationInSeconds}' sec");
                return Task.Delay(TimeSpan.FromSeconds(AwaitableTaskDurationInSeconds));
            }).ContinueWith(previousTask => TaskHelper.OnTerminatedTask(previousTask, AwaitableTaskName));

            var fireAndForgetTask = Task.Run(async () =>
            {
                Logger.LogInformation($"Running '{FireAndForgetTaskName}' during '{FireAndForgetTaskDurationInSeconds}' sec");
                await Task.Delay(TimeSpan.FromSeconds(FireAndForgetTaskDurationInSeconds));
                throw TaskException.TaskHasFailed(FireAndForgetTaskName);
            }).ContinueWith(previousTask => TaskHelper.OnTerminatedTask(previousTask, FireAndForgetTaskName));

            TaskHelper.FireAndForgetTask(fireAndForgetTask, FireAndForgetTaskName);
            return awaitableTask;
        }
    }
}
