using RealStateApp.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Domain.Entities
{
    public class FavoriteProperty : BaseEntity
    {
        public int FavoritePropertyId { get; set; }
        public required string UserId { get; set; }
        public required int PropertyId { get; set; }
        public virtual Property? Property { get; set; }

    }
}
