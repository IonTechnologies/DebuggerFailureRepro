using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using debug_failure.Data;
using debug_failure.Areas.Identity.Data;

namespace debug_failure.Services
{
    public class CreateRolesOnStartupService : IHostedService
{
    // We need to inject the IServiceProvider so we can create 
    // the scoped DatabaseContext
    private readonly IServiceProvider _serviceProvider;
    public CreateRolesOnStartupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Create a new scope to retrieve scoped services
        using(var scope = _serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { "Admin" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database:
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            
            // Clear out existing users
            dbContext.RemoveRange(dbContext.UserData);
            dbContext.RemoveRange(dbContext.Users);
            await dbContext.SaveChangesAsync();

            var user = new User {
                UserName = "user@example.com",
                Email = "user@example.com",
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, "password1");
            if(result.Succeeded == false) {
                throw new Exception("Couldn't create the admin user:\n" + string.Join("\n", result.Errors.Select(e => e.Description)));
            }
            await userManager.AddToRoleAsync(user, "Admin");

            dbContext.Add(new UserData {
                Owner = user,
                Data = "Testing"
            });
            dbContext.Add(new UserData {
                Owner = user,
                Data = "Testing2"
            });

            await dbContext.SaveChangesAsync();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

}