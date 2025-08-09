using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels
{
    public class FavoritePropertyViewModel : BaseEntity
    {
        public int FavoritePropertyId { get; set; }
        public required string UserId { get; set; }
        public required int PropertyId { get; set; }
        public virtual PropertyViewModel? Property { get; set; }

    }
}
