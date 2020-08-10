using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bugsnag.AspNet.Core;
using Microsoft.AspNetCore.DataProtection;
using debug_failure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using debug_failure.Services;

namespace debug_failure
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpClient("mailgun");
            services.AddOptions();

            services.Configure<MailgunOptions>(Configuration.GetSection("Mailgun"));

            services.AddBugsnag(configuration => {
                configuration.ApiKey = Configuration.GetValue<string>("BugsnagToken");
                configuration.ReleaseStage = Environment.EnvironmentName;
            });

            services.AddDbContext<DataProtectionKeysContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DataProtectionKeysConnection")));

            services.AddDataProtection()
                .SetApplicationName("debug_failure")
                .PersistKeysToDbContext<DataProtectionKeysContext>();

            services.AddDbContext<ApplicationDataContext>(options =>
                options.UseLazyLoadingProxies()
                    .UseNpgsql(Configuration.GetConnectionString("applicationConnection")));

            services.AddTransient<IEmailSender, MailgunEmailSender>();

            services.AddHostedService<CreateRolesOnStartupService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
