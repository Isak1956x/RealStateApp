using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Login
{
    public class LoginVM
    {
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
