using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Application.ViewModels
{
    public class ChatViewModel : BaseEntity
    {
        public int ChatId { get; set; }
        public int PropertyId { get; set; }
        public PropertyViewModel? Property { get; set; }
        public required string CustomerId { get; set; }
        public required string AgentId { get; set; }
        public ICollection<MessageViewModel>? Messages { get; set; }
    }
}
