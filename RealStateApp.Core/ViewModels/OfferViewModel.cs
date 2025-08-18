using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Core.Application.ViewModels
{
    public class OfferViewModel 
    {

        public int Id { get; set; }

        public required string ClientId { get; set; }
        public string? ClientName { get; set; } 
        public int PropertyId { get; set; }
        public PropertyViewModel? Property { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; } // "Pending", "Accepted", "Rejected"

    }
}
