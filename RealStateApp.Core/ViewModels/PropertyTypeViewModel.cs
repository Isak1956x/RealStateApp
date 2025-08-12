using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.ViewModels
{
    public class PropertyTypeViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<PropertyViewModel>? Properties { get; set; }
    }
}
