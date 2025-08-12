using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Application.ViewModels
{
    public class PropertyViewModel : BaseEntity
    {
        public int Id { get; set; }
        public required string Code { get; set; }

        public required int PropertyTypeId { get; set; }
        public PropertyTypeViewModel? PropertyType { get; set; }

        public int SaleTypeId { get; set; }
        public SaleTypeViewModel? SaleType { get; set; }

        public decimal Price { get; set; }
        public decimal SizeInMeters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }

        public required string AgentId { get; set; }

        public ICollection<PropertyImprovementViewModel>? PropertyImprovements { get; set; }
        public ICollection<PropertyImageViewModel>? Images { get; set; }
        public ICollection<OfferViewModel>? Offers { get; set; }
        public ICollection<ChatViewModel>? Chats { get; set; }
    }
}
