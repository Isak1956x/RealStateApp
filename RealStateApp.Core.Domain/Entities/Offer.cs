using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Core.Domain.Entities
{
    public class Offer 
    {
      
            public int Id { get; set; }

            public required string ClientId { get; set; }

            public int PropertyId { get; set; }
            public Property? Property { get; set; }

            public decimal Amount { get; set; }
            public DateTime Date { get; set; } = DateTime.UtcNow;
        public Status Status { get; set; } // "Pending", "Accepted", "Rejected"

    }
}
