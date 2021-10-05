using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace GSK.HealthProfessional.WebApp.Configuration
{
    public static class Globalization
    {
        public static IApplicationBuilder UseGlobalizationConfig(this IApplicationBuilder app)
        {
            var defaultCulture = new CultureInfo("pt-BR");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };

            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = defaultCulture;
            app.UseRequestLocalization(localizationOptions);

            return app;
        }
    }
}
