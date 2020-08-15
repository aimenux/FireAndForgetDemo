using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FireAndForgetDemo.Examples
{
    public class Example1 : IExample
    {
        private readonly Settings _settings;
        private readonly ILogger _logger;

        public Example1(IOptions<Settings> options, ILogger logger)
        {
            _logger = logger;
            _settings = options.Value;
        }

        public string Description { get; } = "Run successful Independent tasks";

        public Task RunAsync()
        {
            var awaitableTaskName = _settings.AwaitableTaskName;
            var awaitableTaskDuration = _settings.AwaitableTaskDurationInSeconds;

            var awaitableTask = Task.Run(() =>
            {
                _logger.LogInformation($"Running '{awaitableTaskName}' during '{awaitableTaskDuration}' sec");
                return Task.Delay(TimeSpan.FromSeconds(awaitableTaskDuration));
            }).ContinueWith(currentTask => _logger.LogInformation($"Task '{awaitableTaskName}' is terminated"));

            var fireAndForgetTaskName = _settings.FireAndForgetTaskName;
            var fireAndForgetTaskDuration = _settings.FireAndForgetTaskDurationInSeconds;

            var fireAndForgetTask = Task.Run(() =>
            {
                _logger.LogInformation($"Running '{fireAndForgetTaskName}' during '{fireAndForgetTaskDuration}' sec");
                return Task.Delay(TimeSpan.FromSeconds(fireAndForgetTaskDuration));
            }).ContinueWith(currentTask => _logger.LogInformation($"Task '{fireAndForgetTaskName}' is terminated"));

            fireAndForgetTask.FireAndForgetTask(nameof(fireAndForgetTask));
            return awaitableTask;
        }
    }
}
