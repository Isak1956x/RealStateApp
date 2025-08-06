using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Application.DTOs
{
    public class ChatDto : BaseEntity
    {
        public int ChatId { get; set; }
        public int PropertyId { get; set; }
        public PropertyDto? Property { get; set; }
        public required string CustomerId { get; set; }
        public required string AgentId { get; set; }
        public ICollection<MessageDto>? Messages { get; set; }
    }
}
