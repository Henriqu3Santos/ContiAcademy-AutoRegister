
using System.IO;
using GSK.HealthProfessional.WebApp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GSK.HealthProfessional.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(hostEnvironment.ContentRootPath)
              .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
              .AddJsonFile("appsettings.json", true, true)
              .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ResolveDependencies();
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .Build());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,  ILoggerFactory loggerFactory)
        {
            app.UseGlobalizationConfig();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            if( Configuration.GetSection("RedirectHttps").Get<bool>() )
            {
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();            
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ProfessionalRegistration}/{action=Index}/{id?}");
            });
        }
    }
}
