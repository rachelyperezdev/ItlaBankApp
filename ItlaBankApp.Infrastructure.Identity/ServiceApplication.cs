using ItlaBankApp.Infrastructure.Identity.Entities;
using ItlaBankApp.Infrastructure.Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ItlaBankApp.Infrastructure.Identity.IoC
{
    public static class ServiceApplication
    {
        public static async Task AddIdentitySeeds(this IServiceProvider services)
        {
            #region "Identity Seeds"
            using (var scope = services.CreateScope())
            {
                var serviceScope = scope.ServiceProvider;

                try
                {
                    var userManager = serviceScope.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = serviceScope.GetRequiredService<RoleManager<IdentityRole>>();


                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    await DefaultAdmin.SeedAsync(userManager, roleManager);
                    await DefaultClient.SeedAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
        }
    }
}
