using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.DTOs
{
    public class PropertyImageDto
    {
        public int Id { get; set; }
        public required string Url { get; set; }

        public int PropertyId { get; set; }
        public PropertyDto? Property { get; set; }
    }
}
