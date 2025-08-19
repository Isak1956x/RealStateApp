using RealStateApp.Core.Application.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.PropertyImprovment
{
    public class ImprovmentWritteVM : BaseWritteVM<int>
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
    }
}
