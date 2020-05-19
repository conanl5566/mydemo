using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using abphangfireJob.RecurringJobs;

namespace abphangfireJob
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AppModule>();
            var Configuration = services.GetConfiguration();

            GlobalStateHandlers.Handlers.Add(new SucceededStateExpireHandler(int.Parse(Configuration["Hangfire:JobExpirationTimeout"])));
            services.AddHostedService<RecurringJobsService>();
            services.AddHangfire(x =>
            {
                var connectionString = Configuration["Hangfire:Redis:ConnectionString"];
                x.UseRedisStorage(connectionString, new RedisStorageOptions()
                {
                    //活动服务器超时时间
                    InvisibilityTimeout = TimeSpan.FromMinutes(60),
                    Db = int.Parse(Configuration["Hangfire:Redis:Db"])
                });
                x.UseDashboardMetric(DashboardMetrics.ServerCount)
                    .UseDashboardMetric(DashboardMetrics.RecurringJobCount)
                    .UseDashboardMetric(DashboardMetrics.RetriesCount)
                    .UseDashboardMetric(DashboardMetrics.AwaitingCount)
                    .UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount)
                    .UseDashboardMetric(DashboardMetrics.ScheduledCount)
                    .UseDashboardMetric(DashboardMetrics.ProcessingCount)
                    .UseDashboardMetric(DashboardMetrics.SucceededCount)
                    .UseDashboardMetric(DashboardMetrics.FailedCount)
                       .UseDashboardMetric(DashboardMetrics.EnqueuedCountOrNull)
                          .UseDashboardMetric(DashboardMetrics.FailedCountOrNull)
                    .UseDashboardMetric(DashboardMetrics.DeletedCount);
            });
        }

        public void Configure(IApplicationBuilder app, IConfiguration Configuration)
        {
            app.InitializeApplication();
            app.UseAuthorization();
            var filter = new BasicAuthAuthorizationFilter(
        new BasicAuthAuthorizationFilterOptions
        {
            SslRedirect = false,
            RequireSsl = false,
            LoginCaseSensitive = false,
            Users = new[]
            {
                        new BasicAuthAuthorizationUser
                        {
                            Login = Configuration["Hangfire:Login"] ,
                            PasswordClear= Configuration["Hangfire:PasswordClear"]
                        }
            }
        });
            app.UseHangfireDashboard("", new DashboardOptions
            {
                Authorization = new[]
                {
                   filter
                },
            });
            var jobOptions = new BackgroundJobServerOptions
            {
                Queues = new[] { "critical", "test", "default" },
                WorkerCount = Environment.ProcessorCount * int.Parse(Configuration["Hangfire:ProcessorCount"]),
                ServerName = Configuration["Hangfire:ServerName"],
                SchedulePollingInterval = TimeSpan.FromSeconds(1), //计划轮询间隔  支持任务到秒
            };
            app.UseHangfireServer(jobOptions);
        }
    }
}