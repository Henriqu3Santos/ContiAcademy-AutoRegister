using GSK.HealthProfessional.Data;
using GSK.HealthProfessional.Service;
using GSK.HealthProfessional.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GSK.HealthProfessional.WebApp.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddScoped<AuthenticationApiService>();
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
            services.AddScoped<IOccupationAreaService, OccupationAreaService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IProfessionalService, ProfessionalService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<INotifier, Notificador>();

            return services;
        }

        
    }
}
