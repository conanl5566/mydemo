using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<AppDbContext>();

            services.AddCap(x =>
            {
                x.UseEntityFramework<AppDbContext>();
                //  x.UseSqlServer("Server=127.0.0.1;Database=DNCDB_Order;Uid=sa;Password=P@ssw0rd"); // SQL Server
                // x.UseRabbitMQ("host=127.0.0.1:5672;virtualHost=/;username=guest;password=guest");

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = "127.0.0.1";
                    cfg.VirtualHost = "/";
                    cfg.Port = 5672;
                    cfg.UserName = "guest";
                    cfg.Password = "guest";
                }); // RabbitMQ

                x.UseDashboard();
                x.FailedRetryCount = 5;
                x.FailedThresholdCallback = (type, name, content) =>
                {
                    Console.WriteLine($@"A message of type {type} failed after executing {x.FailedRetryCount} several times, requiring manual troubleshooting. Message name: {name}, message body: {content}");
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}