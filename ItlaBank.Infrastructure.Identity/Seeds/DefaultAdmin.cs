using ItlaBankApp.Infrastructure.Identity.Entities;
using ItlaBankApp.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;

namespace ItlaBankApp.Infrastructure.Identity.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultAdmin = new();
            defaultAdmin.UserName = "basicAdmin";
            defaultAdmin.Email = "basicadmin@email.com";
            defaultAdmin.FirstName = "Jane";
            defaultAdmin.LastName = "Smith";
            defaultAdmin.PhoneNumber = "829-222-2222";
            defaultAdmin.EmailConfirmed = true;
            defaultAdmin.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != defaultAdmin.Id))
            {
                var user = await userManager.FindByNameAsync(defaultAdmin.UserName);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultAdmin, "456Pa$$wOrD++");
                    await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
                }
            }
        }
    }
}
