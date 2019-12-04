using System;
using Hangfire;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.DependencyInjection;

namespace demo3 {
    public class SampleJobCreator : ITransientDependency {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public SampleJobCreator (IBackgroundJobManager backgroundJobManager) {
            _backgroundJobManager = backgroundJobManager;
        }

        public void CreateJobs () {
            _backgroundJobManager.Enqueue (new WriteToConsoleGreenJobArgs { Value = "test 1 (green)" }, BackgroundJobPriority.Low, System.TimeSpan.FromSeconds (30));
            _backgroundJobManager.Enqueue (new WriteToConsoleGreenJobArgs { Value = "test 2 (green)" }, BackgroundJobPriority.High, System.TimeSpan.FromSeconds (32));
            _backgroundJobManager.Enqueue (new WriteToConsoleYellowJobArgs { Value = "test 1 (yellow)" }, BackgroundJobPriority.Low, System.TimeSpan.FromSeconds (40));
            _backgroundJobManager.Enqueue (new WriteToConsoleYellowJobArgs { Value = "test 2 (yellow)" }, BackgroundJobPriority.High, System.TimeSpan.FromSeconds (52));

            //支持基于队列的任务处理：任务执行不是同步的，而是放到一个持久化队列中，以便马上把请求控制权返回给调用者。
            var jobId = BackgroundJob.Enqueue (() => Console.WriteLine ("队列任务"));
            BackgroundJob.Enqueue<HangfireJobExecutionAdapter<WriteToConsoleGreenJobArgs>> (
                adapter => adapter.Execute (new WriteToConsoleGreenJobArgs { Value = "队列任务 2" })
            );

            //延迟任务执行：不是马上调用方法，而是设定一个未来时间点再来执行。
            BackgroundJob.Schedule (() => Console.WriteLine ("延时任务"), TimeSpan.FromSeconds (10));

            BackgroundJob.Schedule<HangfireJobExecutionAdapter<WriteToConsoleYellowJobArgs>> (
                adapter => adapter.Execute (new WriteToConsoleYellowJobArgs { Value = "延时任务 2" }), TimeSpan.FromSeconds (10));

            //循环任务执行：一行代码添加重复执行的任务，其内置了常见的时间循环模式，也可基于CRON表达式来设定复杂的模式。
            RecurringJob.AddOrUpdate ("周期任务的名字", () => Console.WriteLine ("每分钟执行任务"), Cron.Minutely); //注意最小单位是分钟

            RecurringJob.AddOrUpdate<HangfireJobExecutionAdapter<WriteToConsoleYellowJobArgs>> (
                adapter => adapter.Execute (new WriteToConsoleYellowJobArgs { Value = "每分钟执行任务 2" }), Cron.Minutely);

            //延续性任务执行：类似于.NET中的Task,可以在第一个任务执行完之后紧接着再次执行另外的任务
            BackgroundJob.ContinueJobWith (jobId, () => Console.WriteLine ("连续任务"));

            BackgroundJob.ContinueJobWith<HangfireJobExecutionAdapter<WriteToConsoleGreenJobArgs>> (jobId,
                adapter => adapter.Execute (new WriteToConsoleGreenJobArgs { Value = "连续任务 2" }));

            //删除指定的周期性任务
            RecurringJob.RemoveIfExists ("周期任务的名字");
            //立即执行周期性任务：
            RecurringJob.Trigger ("周期任务的名字");
        }
    }
}