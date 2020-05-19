using Autofac;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.Redis;
using hangfiretest.RecurringJobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using TestService;

namespace hangfiretest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblys = System.Reflection.Assembly.Load("TestService");
            var baseType = typeof(IDependency);
            builder.RegisterAssemblyTypes(assemblys)
             .Where(m => baseType.IsAssignableFrom(m) && m != baseType).PropertiesAutowired()
             .AsImplementedInterfaces().InstancePerDependency();

            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddControllersAsServices();
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
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