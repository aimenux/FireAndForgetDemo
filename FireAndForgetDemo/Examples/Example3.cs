using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireAndForgetDemo.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FireAndForgetDemo.Examples
{
    public class Example3 : AbstractExample
    {
        private readonly IStubHelper _stubHelper;

        public Example3(IOptions<Settings> options, ITaskHelper taskHelper, IStubHelper stubHelper, ILogger logger) : base(options.Value, taskHelper, logger)
        {
            _stubHelper = stubHelper;
        }

        public override string Description { get; } = "Run retry/wait fire/forget task";

        public override async Task RunAsync()
        {
            var stableFindTask = _stubHelper.GetDataFromStableStorageAsync();
            var stableFoundDocuments = await stableFindTask;

            var unstableFindTask = _stubHelper.GetDataFromUnstableStorageAsync()
                .ContinueWith(currentTask =>
                {
                    var unstableFoundDocuments = currentTask.GetAwaiter().GetResult();
                    CheckDataConsistency(stableFoundDocuments, unstableFoundDocuments);
                });

            TaskHelper.FireAndForgetTask(unstableFindTask, FireAndForgetTaskName);

            PrintStableStorageResults(stableFoundDocuments);
        }

        private void CheckDataConsistency(ICollection<int> stableFoundDocuments, ICollection<int> unstableFoundDocuments)
        {
            if (!stableFoundDocuments.Any())
            {
                Logger.LogWarning("Unexpected behaviour: unfound documents in stable storage");
            }

            foreach (var document in stableFoundDocuments)
            {
                if (!unstableFoundDocuments.Contains(document))
                {
                    Logger.LogWarning($"Unfound document '{document}' in unstable storage");
                }
            }
        }

        private void PrintStableStorageResults(ICollection<int> stableFoundDocuments)
        {
            Logger.LogTrace($"Found '{stableFoundDocuments.Count}' document(s) in stable storage");
        }
    }
}
