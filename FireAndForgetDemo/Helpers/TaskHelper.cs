using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FireAndForgetDemo.Helpers
{
    public class TaskHelper : ITaskHelper
    {
        private readonly ILogger _logger;

        public TaskHelper(ILogger logger)
        {
            _logger = logger;
        }
        
        public void FireAndForgetTask(Task fireForgetTask, string fireForgetTaskName = null, [CallerMemberName] string callerName = null)
        {
            void OnFaultedTask(Task faultedTask) => OnTerminatedTask(faultedTask, fireForgetTaskName);
            Task.Run(() => fireForgetTask).ContinueWith(OnFaultedTask, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void OnTerminatedTask(Task previousTask, string previousTaskName = null, [CallerMemberName] string callerName = null)
        {
            var exception = previousTask.Exception;
            var name = previousTaskName ?? callerName;

            if (previousTask.IsFaulted)
            {
                _logger.LogError($"An exception has occured on previousTask '{name}': {exception}");
                return;
            }

            _logger.LogInformation($"Task '{previousTaskName}' is terminated with status '{previousTask.Status}'");
        }
    }
}