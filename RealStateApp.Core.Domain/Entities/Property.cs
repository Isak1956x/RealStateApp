using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Domain.Entities
{
    public class Property : BaseEntity
    {
        public int Id { get; set; }
        public required string Code { get; set; }

        public required int PropertyTypeId { get; set; }
        public PropertyType? PropertyType { get; set; }

        public int SaleTypeId { get; set; }
        public SaleType? SaleType { get; set; }

        public decimal Price { get; set; }
        public decimal SizeInMeters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }

        public required string AgentId { get; set; }

        public ICollection<PropertyImprovement>? PropertyImprovements { get; set; }
        public ICollection<PropertyImage>? Images { get; set; }
        public ICollection<Offer>? Offers { get; set; }
        public ICollection<Chat>? Chats { get; set; }   
    }
}
