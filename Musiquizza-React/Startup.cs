using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Amazon.XRay.Recorder.Handlers.AspNetCore;
using Amazon.XRay.Recorder.Core;
using System.Xml.Linq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Options;
using AspNet.Security.OAuth.Spotify;
using AspNet.Security.OAuth;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Musiquizza_React
{
    public class Startup
    {
        private readonly string ConfigSection = "AWSConfiguration";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AWSXRayRecorder.InitializeInstance(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions()); //options are in appsettings under AWS
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddTransient<SongService>();
            services.AddTransient<SpotifyService>();
            
             services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                });
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);;
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/signout";
            })
            .AddSpotify(options => {
                // options.ClientId = System.Environment.GetEnvironmentVariable("SpotifyClientId");
                // options.ClientSecret = System.Environment.GetEnvironmentVariable("SpotifyClientSecret");
                options.ClientId = "cd63690c687f48538e3f7e6b38ecd8f6";
                options.ClientSecret = "1643da7651574c358bb9414eb76be8e3";
                options.Scope.Add("streaming");
                options.Scope.Add("user-read-private");
                options.Scope.Add("user-read-email");
                options.Scope.Add("user-read-birthdate");
                options.Scope.Add("user-modify-playback-state");
                options.SaveTokens = true;
                
            

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowAllOrigins");

            app.UseHsts();

            app.UseHttpsRedirection();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseXRay("Musiquizza_React");

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();
            //app.UseMvc();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");

                // routes.MapSpaFallbackRoute(
                //     name: "spa-fallback",
                //     defaults: new { controller = "Authentication", action = "SignIn" });
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
               
            });

            
        }
    }
}
