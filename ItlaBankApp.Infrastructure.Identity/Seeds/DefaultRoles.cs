using ItlaBankApp.Infrastructure.Identity.Entities;
using ItlaBankApp.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;

namespace ItlaBankApp.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Client.ToString()));
        }
    }
}