using RealStateApp.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.DTOs
{
    public class FavoritePropertyDto : BaseEntity
    {
        public int FavoritePropertyId { get; set; }
        public required string UserId { get; set; }
        public required int PropertyId { get; set; }
        public virtual PropertyDto? Property { get; set; }

    }
}
