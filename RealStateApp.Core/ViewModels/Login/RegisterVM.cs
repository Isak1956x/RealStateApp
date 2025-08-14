using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Login
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        public int RoleId { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Photo is required.")]
        [DataType(DataType.Upload)]
        public IFormFile PhotoPath { get; set; }

        [Required(ErrorMessage = "Id Number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Id Number must be 11 digits.")]
        public string? IdNumber { get; set; } 

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Required(ErrorMessage = "Phone Number is required.")]
        public string? PhoneNumber { get; set; }
    }
}
