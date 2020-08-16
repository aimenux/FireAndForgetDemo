using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireAndForgetDemo.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace FireAndForgetDemo.Helpers
{
    public class StubHelper : IStubHelper
    {
        private const int Retry = 5;
        private const int WaitBeforeRetry = 100;
        private static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());

        private readonly Settings _settings;
        private readonly ILogger _logger;

        public StubHelper(IOptions<Settings> options, ILogger logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<ICollection<int>> GetDataFromStableStorageAsync()
        {
            var foundDocuments = await FindAsync(TimeSpan.FromSeconds(_settings.AwaitableTaskDurationInSeconds));
            _logger.LogInformation($"Task '{_settings.AwaitableTaskName}' is terminated");
            return foundDocuments;
        }

        public async Task<ICollection<int>> GetDataFromUnstableStorageAsync()
        {
            var findTask = FindAsync(TimeSpan.FromSeconds(_settings.FireAndForgetTaskDurationInSeconds))
                .ContinueWith(async currentTask =>
                {
                    var foundDocuments = await currentTask;
                    if (!_settings.FoundDocumentsOnUnstableStorage || !foundDocuments.Any())
                    {
                        throw StubException.UnfoundDocuments();
                    }

                    return foundDocuments;
                }).Unwrap();

            var documents = await Policy.Handle<StubException>()
                .WaitAndRetryAsync(Retry, OnWaitBeforeRetry(WaitBeforeRetry), OnGetRetry())
                .ExecuteAsync(() => findTask);

            _logger.LogInformation($"Task '{_settings.FireAndForgetTaskName}' is terminated");

            return documents;
        }

        private static async Task<ICollection<int>> FindAsync(TimeSpan delay)
        {
            await Task.Delay(delay);

            const int nbrDocuments = 5;
            var documents = Enumerable.Range(0, nbrDocuments)
                .Select(x => Random.Next(100, 500))
                .ToList();

            return documents;
        }

        private static Func<int, TimeSpan> OnWaitBeforeRetry(int waitBeforeRetry)
        {
            return _ => TimeSpan.FromMilliseconds(waitBeforeRetry);
        }

        private Action<Exception, TimeSpan, int, Context> OnGetRetry()
        {
            return (exception, waitTimespan, retryCount, context) =>
            {
                _logger.LogWarning("Failed to get documents on stub storage ({retryCount})", retryCount);
            };
        }
    }
}