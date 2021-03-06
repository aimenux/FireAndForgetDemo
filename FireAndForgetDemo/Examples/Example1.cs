﻿using System;
using System.Threading.Tasks;
using FireAndForgetDemo.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FireAndForgetDemo.Examples
{
    public class Example1 : AbstractExample
    {
        public Example1(IOptions<Settings> options, ITaskHelper taskHelper, ILogger logger) : base(options.Value, taskHelper, logger)
        {
        }

        public override string Description { get; } = "Run successful fire/forget task";

        public override Task RunAsync()
        {
            var awaitableTask = Task.Run(() =>
            {
                Logger.LogInformation($"Running '{AwaitableTaskName}' during '{AwaitableTaskDurationInSeconds}' sec");
                return Task.Delay(TimeSpan.FromSeconds(AwaitableTaskDurationInSeconds));
            }).ContinueWith(previousTask => TaskHelper.OnTerminatedTask(previousTask, AwaitableTaskName));

            var fireAndForgetTask = Task.Run(() =>
            {
                Logger.LogInformation($"Running '{FireAndForgetTaskName}' during '{FireAndForgetTaskDurationInSeconds}' sec");
                return Task.Delay(TimeSpan.FromSeconds(FireAndForgetTaskDurationInSeconds));
            }).ContinueWith(previousTask => TaskHelper.OnTerminatedTask(previousTask, FireAndForgetTaskName));

            TaskHelper.FireAndForgetTask(fireAndForgetTask, FireAndForgetTaskName);
            return awaitableTask;
        }
    }
}
