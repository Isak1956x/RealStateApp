namespace RealStateApp.Core.Application.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdCardNumber { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; } // Assuming Role is a string, adjust as necessary,
        public string PhotoPath { get; set; }
        public string PhoneNumber { get; set; }

    }
}
