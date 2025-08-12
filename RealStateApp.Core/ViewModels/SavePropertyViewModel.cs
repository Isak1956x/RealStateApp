using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.ViewModels
{
    public class SavePropertyViewModel : BaseEntity
    {
       
        public string? Code { get; set; }
        public required  int Id { get; set; }
        [Required(ErrorMessage = "Property Type is required.")]
        public required int PropertyTypeId { get; set; }
        public List<PropertyTypeViewModel>? PropertyTypes { get; set; }
        [Required(ErrorMessage = "Sale Type is required.")]
        public int SaleTypeId { get; set; }
        public List<SaleTypeViewModel>? SaleTypes { get; set; }
        [Required(ErrorMessage = "Improvement is required.")]
        public int ImprovementId { get; set; }
        public List<ImprovementViewModel>? Improvements { get; set; }
        [Required(ErrorMessage = "A image  is required")]
        public List<IFormFile> Images { get; set; } = new();
        public PropertyViewModel? Property { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Size is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Size must be greater than zero.")]
        public decimal SizeInMeters { get; set; }
        [Required(ErrorMessage = "Bedroom is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Bedroom must be greater than 1.")]
        public int Bedrooms { get; set; }
        [Required(ErrorMessage = "Bathrooms is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Bathrooms must be greater than 1.")]
        public int Bathrooms { get; set; }
        [Required(ErrorMessage = "Description is required.")]
     [DataType(DataType.MultilineText)]
        public required string Description { get; set; }
        public bool IsAvailable { get; set; }

        public required string AgentId { get; set; }

    }
}
