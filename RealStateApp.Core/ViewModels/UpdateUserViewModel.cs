using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealStateApp.Core.Application.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public required string LastName { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile? Photo { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Required(ErrorMessage = "Phone Number is required.")]
        public required string PhoneNumber { get; set; }
        public string? PhotoPath { get; set; } 

    }
}
