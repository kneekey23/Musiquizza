using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AWSAppService;
using AWSAppService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Admin
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
            services.AddMvc();
            services.AddOptions();

            // Register the IConfiguration instance which MyOptions binds against.
            services.Configure<AWSOptions>(Configuration.GetSection("AWSConfiguration"));
            Amazon.RegionEndpoint AppRegion = Amazon.RegionEndpoint.GetBySystemName(Configuration.GetSection("AWSConfiguration").Key("Region"));
            //services.AddScoped<IAWSAppService>(s => new DBDataService("MyConnectionString"));
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Admin}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "api",
                    template: "api/{controller}/{id?}",
                    defaults: new { controller = "Admin", action = "" });
            });
        }
    }
}
