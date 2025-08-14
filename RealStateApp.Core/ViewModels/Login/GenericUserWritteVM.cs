using RealStateApp.Core.Application.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Login
{
    public class GenericUserWritteVM : BaseWritteVM<string>
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "IdCardNumber is required.")]
        [RegularExpression(@"^\d{8,10}$", ErrorMessage = "IdCardNumber must be between 8 and 10 digits.")]
        public string IdCardNumber { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public required int Role { get; set; } 
    }
}
