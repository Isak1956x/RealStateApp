namespace RealStateApp.Core.Application.ViewModels.Login
{
    public class GenericUserReadVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdCardNumber { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }

        public int LinkedProperties { get; set; }
    }
}
