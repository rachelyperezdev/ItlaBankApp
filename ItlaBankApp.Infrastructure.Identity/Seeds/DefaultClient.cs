using ItlaBankApp.Infrastructure.Identity.Entities;
using ItlaBankApp.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;

namespace ItlaBankApp.Infrastructure.Identity.Seeds
{
    public static class DefaultClient
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultClient = new();
            defaultClient.UserName = "defaultClient";
            defaultClient.Email = "defaultclient@email.com";
            defaultClient.FirstName = "Emily";
            defaultClient.LastName = "Johnson";
            defaultClient.PhoneNumber = "849-333-3333";
            defaultClient.IdentificationCard = "402-9876543-2";
            defaultClient.PhoneNumberConfirmed = true;
            defaultClient.EmailConfirmed = true;
            defaultClient.IsActive = true;

            if (userManager.Users.All(u => u.Id != defaultClient.Id))
            {
                var user = await userManager.FindByNameAsync(defaultClient.UserName);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultClient, "987Pa$$wOrD++");
                    await userManager.AddToRoleAsync(defaultClient, Roles.Client.ToString());
                }
            }

        }
    }
}