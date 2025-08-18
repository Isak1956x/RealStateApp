using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Infraestructure.Identity.Entities;


namespace RealStateApp.Infraestructure.Identity.Seeds
{
    public static class DefaultAgentUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            AppUser user = new()
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "agent@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "agent",
                IdNumber = "1234",
                PhotoPath = "https://www.realestate.com.au/news-image/w_1280,h_720/v1681361659/stock-assets/editorial-use-only/joshtesolin.jpg?_i=AA",
                IsActive = true
            };

            if (await userManager.Users.AllAsync(u => u.Id != user.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(user.Email);
                if (entityUser == null)
                {
                    await userManager.CreateAsync(user, "123Pa$$word!");
                    await userManager.AddToRoleAsync(user, UserRoles.Agent.ToString());
                }
            }

        }
    }
}
