namespace RealStateApp.Core.Application.ViewModels.Login
{
    public class UserGenericList
    {
        public IEnumerable<GenericUserReadVM> List { get; set; }
        public string UserType { get; set; } = "Admin"; // Default value
        public string Controller { get; set; }
    }
}
