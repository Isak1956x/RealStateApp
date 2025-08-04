using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Core.Domain.Entities
{
    public class Offer
    {
      
            public int Id { get; set; }

            public int ClientId { get; set; }

            public int PropertyId { get; set; }
            public Property? Property { get; set; }

            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public Status Status { get; set; } // "Pending", "Accepted", "Rejected"

    }
}
