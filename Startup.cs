using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PFStudio.PFSign.Converters;
using PFStudio.PFSign.Data;

namespace PFStudio.PFSign
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
            // 注册数据库上下文
            // 务必在AddMvc之前添加
            services.AddDbContext<RecordDbContext>(options
               => options.UseMySQL(Configuration.GetConnectionString("MySQL")));

            ConfigureBaseServices(services);
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // 注册数据库上下文
            // 务必在AddMvc之前添加
            services.AddDbContext<RecordDbContext>(options
               => options.UseMySQL(Configuration.GetConnectionString("Local")));

            ConfigureBaseServices(services);
        }

        private static void ConfigureBaseServices(IServiceCollection services)
        {
            services.AddMvc();

            // 注册TimeJob服务
            services.AddTimedJob();

            // 注册本地时间转换器
            services.AddTransient(s =>
            {
                // 配置Json转换设置
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                // 添加转换本地时间的转换器
                serializerSettings.Converters.Add(new LocalDateTimeConverter());

                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                return serializerSettings;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseTimedJob();

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
