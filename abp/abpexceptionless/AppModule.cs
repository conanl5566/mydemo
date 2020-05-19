using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace abpexceptionless
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(AbpAutofacModule))] //Add dependency to ABP Autofac module
    public class AppModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseMvcWithDefaultRouteAndArea();
            var configuration = context.GetConfiguration();
            ExceptionlessClient.Default.Configuration.ApiKey = configuration.GetSection("Exceptionless:ApiKey").Value;
            //自己搭建的 Exceptionless 则配置对应的url
            //  ExceptionlessClient.Default.Configuration.ServerUrl = configuration.GetSection("Exceptionless:ServerUrl").Value;
            app.UseExceptionless();
        }
    }
}