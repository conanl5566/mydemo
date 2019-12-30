using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace WebApplicationHealthCheck
{
    //https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
    //https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private DriveInfo[] _drives = DriveInfo.GetDrives();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var testDrive = _drives.FirstOrDefault(d => d.DriveType == DriveType.Fixed);
            services.AddHealthChecks()
                     //System
                     .AddPrivateMemoryHealthCheck(1000_000_000L) //最大私有内存不超过1GB
                     .AddVirtualMemorySizeHealthCheck(1000_000_000L) //最大虚拟内存不超过1GB
                     .AddWorkingSetHealthCheck(1000_000_000L)//最大工作内存不超过1GB
                     .AddDiskStorageHealthCheck(x => x.AddDrive(testDrive.Name, 1000L)) //C盘需要超过1GB自由空间
                    ;
            services.AddHealthChecksUI();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app
         .UseRouting()
         .UseEndpoints(config =>
         {
             config.MapHealthChecksUI();
         });
            app.UseHealthChecks("/health",
                 new HealthCheckOptions
                 {
                     ResponseWriter = async (context, report) =>
                     {
                         var result = JsonConvert.SerializeObject(
                             new
                             {
                                 status = report.Status.ToString(),
                                 errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                             });
                         context.Response.ContentType = MediaTypeNames.Application.Json;
                         await context.Response.WriteAsync(result);
                     }
                 });
        }
    }
}