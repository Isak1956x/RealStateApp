

using Microsoft.AspNetCore.Identity;

namespace RealStateApp.Infraestructure.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string PhotoPath { get; set; }
        public bool IsActive { get; set; }
    }
}
