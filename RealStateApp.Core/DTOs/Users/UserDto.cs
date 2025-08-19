namespace RealStateApp.Core.Application.DTOs.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdCardNumber { get; set; }
        public bool IsActive { get; set; }
// Assuming Role is a string, adjust as necessary,
        public string PhotoPath { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; } 
        public int RoleId { get; set; }
        public int LinkedProperties { get; set; }
    }
}
