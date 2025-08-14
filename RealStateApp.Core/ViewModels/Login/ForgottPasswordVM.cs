using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Login
{
    public class ForgottPasswordVM
    {
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "Invalid email addres")]
        public string Email { get; set; }
    }
}
