using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Infraestructure.Identity.Entities;


namespace RealStateApp.Infraestructure.Identity.Seeds
{
    public static class DefaultClientUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            AppUser user = new()
            {
                FirstName = "Isaac",
                LastName = "Jaja",
                Email = "client@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "client",
                IdNumber = "122332",
                PhotoPath = "https://www.realestate.com.au/news-image/w_1280,h_720/v1681361659/stock-assets/editorial-use-only/joshtesolin.jpg?_i=AA",
                IsActive = true
            };

            if (await userManager.Users.AllAsync(u => u.Id != user.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(user.Email);
                if (entityUser == null)
                {
                    await userManager.CreateAsync(user, "123Pa$$word!");
                    await userManager.AddToRoleAsync(user, UserRoles.Client.ToString());
                }
            }

        }
    }
}
