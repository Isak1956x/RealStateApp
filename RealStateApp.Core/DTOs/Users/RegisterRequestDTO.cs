namespace RealStateApp.Core.Application.DTOs.Users
{
    public class RegisterRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string? PhotoPath { get; set; }
        public string? IdCardNumber { get; set; } 
        public string? PhoneNumber { get; set; }

    }
}
