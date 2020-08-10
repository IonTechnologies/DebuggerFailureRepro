using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using debug_failure.Areas.Identity.Data;
using debug_failure.Data;

[assembly: HostingStartup(typeof(debug_failure.Areas.Identity.IdentityHostingStartup))]
namespace debug_failure.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDefaultIdentity<User>(options => {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                    })
                    .AddRoles<IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddEntityFrameworkStores<ApplicationDataContext>();
            });
        }
    }
}
