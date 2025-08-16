using RealStateApp.Core.Application.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.PropertyTypes
{
    public class PropertyTypeWriteVM : BaseWritteVM<int>
    {
        [Required(ErrorMessage = "Name is required.")]  
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
    }
}
