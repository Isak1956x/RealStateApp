using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Domain.Entities
{
    public class Chat : BaseEntity
    {
        public int ChatId { get; set; }
        public int PropertyId {  get; set; }
        public Property? Property { get; set; }
        public required string CustomerId { get; set; }
        public required string AgentId { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
