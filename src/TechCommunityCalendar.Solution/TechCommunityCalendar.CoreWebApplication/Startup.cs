using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication
{
    public class Startup
    {
        IWebHostEnvironment currentEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            currentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string csvPath = Path.Combine(currentEnvironment.WebRootPath, "Data", "TechEvents.csv");
            //string azureConnectionString = "";

            services.AddControllersWithViews();
            //services.AddScoped<ITechEventQueryRepository, FakeTechEventRepository>();
            services.AddScoped<ITechEventQueryRepository>(x => new CsvTechEventRepository(csvPath));
            //services.AddScoped<ITechEventQueryRepository>(x => new AzureTechEventRepository(azureConnectionString));

            services.AddLocalization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            const string gbCultureCode = "en-GB";

            var supportedCultures = new[] {
                new CultureInfo(gbCultureCode)
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(gbCultureCode),
                SupportedCultures = supportedCultures,
                FallBackToParentCultures = false
            });

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(gbCultureCode);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
