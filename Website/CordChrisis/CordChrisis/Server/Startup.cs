using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CordChrisis.Server.Hubs;
using System.Linq;
using Blazored.SessionStorage;
using Blazor.Extensions.Canvas;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace CordChrisis.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("https://youtubewebgl.herokuapp.com", "https://unity-dev-youtube.herokuapp.com");
                });
            });

            services.AddControllersWithViews();
            services.AddBlazoredSessionStorage();
            //services.AddScoped<Blazored.SessionStorage.ISessionStorageService>();

            //services.AddScoped<CordChrisis.Client.Services.SessionStorage>();
            // services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".unityweb"] = "application/octet-stream";
            //app.UseStaticFiles();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseCors("CorsPolicy");
            
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //THIS IS WHERE WE ADD ANY NEW HUBS THAT WE MAKE IN THE "Hubs" FOLDER!
                //--------------------------------------------------------------------------
               // endpoints.MapHub<PlayerHub>("/playerHub");
                //--------------------------------------------------------------------------
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
