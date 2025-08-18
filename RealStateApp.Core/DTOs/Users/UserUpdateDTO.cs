namespace RealStateApp.Core.Application.DTOs.Users
{
    public class UserUpdateDTO
    {
        public  string? Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? PhotoPath { get; set; }
        public string? Password { get; set; }    
    }
}
