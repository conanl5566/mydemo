using demo3;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MySql.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.Modularity;

namespace demoJobs {
    [DependsOn (
        typeof (AbpAutofacModule),
        typeof (AbpBackgroundJobsHangfireModule)
    )]
    public class DemoAppHanfireModule : AbpModule {

        public override void PreConfigureServices (ServiceConfigurationContext context) {
            var configuration = ConfigurationHelper.BuildConfiguration ();
            context.Services.SetConfiguration (configuration);

            string connstr=configuration.GetConnectionString ("DefaultMySql");
            GlobalConfiguration.Configuration.UseStorage (
                new MySqlStorage (connstr));

            context.Services.PreConfigure<IGlobalConfiguration> (hangfireConfiguration => {
                hangfireConfiguration.UseStorage (new MySqlStorage (connstr));
            });

        }
        public override void OnPostApplicationInitialization (Volo.Abp.ApplicationInitializationContext context) {
            context.ServiceProvider
                .GetRequiredService<SampleJobCreator> ()
                .CreateJobs ();
        }
        public override void OnApplicationInitialization (ApplicationInitializationContext context) {
            context
                .ServiceProvider
                .GetRequiredService<ILoggerFactory> ()
                .AddConsole (LogLevel.Debug);
        }
    }
}