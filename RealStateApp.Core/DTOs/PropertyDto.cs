using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.DTOs
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public required string Code { get; set; }

        public required int PropertyTypeId { get; set; }
        public PropertyTypeDto? PropertyType { get; set; }

        public int SaleTypeId { get; set; }
        public SaleTypeDto? SaleType { get; set; }

        public decimal Price { get; set; }
        public decimal SizeInMeters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }

        public required string AgentId { get; set; }

        public ICollection<PropertyImprovementDto>? PropertyImprovements { get; set; }
        public ICollection<PropertyImageDto>? Images { get; set; }
        public ICollection<OfferDto>? Offers { get; set; }
    }
}
