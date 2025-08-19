using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Infraestructure.Identity.Entities;


namespace RealStateApp.Infraestructure.Identity.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            AppUser user = new()
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "addmin@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "admin",
                IdNumber = "1233",
                PhotoPath = "dada",
            };

            if (await userManager.Users.AllAsync(u => u.Id != user.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(user.Email);
                if (entityUser == null)
                {
                    await userManager.CreateAsync(user, "123Pa$$word!");
                    await userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
                }
            }

        }
    }
}
