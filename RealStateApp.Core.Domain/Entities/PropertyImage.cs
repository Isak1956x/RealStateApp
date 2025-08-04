using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Domain.Entities
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public required string Url { get; set; }

        public int PropertyId { get; set; }
        public Property? Property { get; set; }
    }
}
