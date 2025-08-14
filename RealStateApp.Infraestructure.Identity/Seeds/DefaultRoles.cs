using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Infraestructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Agent.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Client.ToString()));
        }
    }
}
