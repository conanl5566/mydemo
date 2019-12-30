using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TestService;

namespace hangfiretest.RecurringJobs
{
    internal class RecurringJobsService : BackgroundService
    {
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IRecurringJobManager _recurringJobs;
        private readonly ILogger<RecurringJobScheduler> _logger;
        public Itest _test { get; set; }

        public RecurringJobsService(
            [NotNull] IBackgroundJobClient backgroundJobs,
            [NotNull] IRecurringJobManager recurringJobs,
            [NotNull] ILogger<RecurringJobScheduler> logger)
        {
            _backgroundJobs = backgroundJobs ?? throw new ArgumentNullException(nameof(backgroundJobs));
            _recurringJobs = recurringJobs ?? throw new ArgumentNullException(nameof(recurringJobs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _recurringJobs.AddOrUpdate<Itest>("seconds", i => _test.demo(), "*/1 * * * * *", queue: "critical");
                //_backgroundJobs.Enqueue<Services>(x => x.LongRunning(JobCancellationToken.Null));
                //_recurringJobs.AddOrUpdate("seconds", () => Console.WriteLine("Hello, seconds!"), "*/15 * * * * *");
                //_recurringJobs.AddOrUpdate("minutely", () => Console.WriteLine("Hello, world!"), Cron.Minutely);
                //_recurringJobs.AddOrUpdate("hourly", () => Console.WriteLine("Hello"), "25 15 * * *");
                //_recurringJobs.AddOrUpdate("neverfires", () => Console.WriteLine("Can only be triggered"), "0 0 31 2 *");
                //_recurringJobs.AddOrUpdate("Hawaiian", () => Console.WriteLine("Hawaiian"), "15 08 * * *", TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"));
                //_recurringJobs.AddOrUpdate("UTC", () => Console.WriteLine("UTC"), "15 18 * * *");
                //_recurringJobs.AddOrUpdate("Russian", () => Console.WriteLine("Russian"), "15 21 * * *", TimeZoneInfo.Local);
            }
            catch (Exception e)
            {
                _logger.LogError("An exception occurred while creating recurring jobs.", e);
            }

            return Task.CompletedTask;
        }
    }
}