using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.IO;
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
            //string csvPath = Path.Combine(currentEnvironment.WebRootPath, "Data", "TechEvents.csv");

            services.AddMemoryCache();
            services.AddControllersWithViews();
            //services.AddScoped<ITechEventQueryRepository, FakeTechEventRepository>();
            //services.AddScoped<ITechEventQueryRepository>(x => new CsvTechEventRepository(csvPath));

            // Will probably have to pass connection string in..
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            

            services.AddScoped<ITechEventQueryRepository>(x => new SqlTechEventRepository(connectionString));
            services.AddScoped<ITechEventAdminRepository>(x => new SqlTechEventRepository(connectionString));

            services.AddLocalization();
            //services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                // Accenture required headings
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubdomains");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("Cache-Control", "public, max-age=31536000");
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self' 'unsafe-eval' 'unsafe-inline' *.techcommunitycalendar.com; script-src *.googletagmanager.com *.unpkg.com; upgrade-insecure-requests; block-all-mixed-content");

                await next.Invoke();
            });

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
