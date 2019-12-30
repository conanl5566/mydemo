using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//https://www.cnblogs.com/liumengchen-boke/p/8337065.html

//https://www.cnblogs.com/thinksjay/p/10787633.html
//https://www.cnblogs.com/jaycewu/p/7791102.html
namespace WebApplication25
{
    public static class Test
    {
        /// <summary>
        /// Adds support for client authentication using JWT bearer assertions.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddJwtBearerClientAuthentication(this IIdentityServerBuilder builder)
        {
            builder.AddSecretParser<JwtBearerClientAssertionSecretParser>();
            builder.AddSecretValidator<PrivateKeyJwtSecretValidator>();

            return builder;
        }
    }

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
            services.AddIdentityServer()
               .AddDeveloperSigningCredential()
               .AddJwtBearerClientAuthentication()
               .AddInMemoryApiResources(Config.GetApiResources())  //配置资源
               .AddInMemoryClients(Config.GetClients())       //配置客户端
               .AddTestUsers(Config.GetTestUsers());            //配置测试用户
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            //使用identityserver中间件
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}