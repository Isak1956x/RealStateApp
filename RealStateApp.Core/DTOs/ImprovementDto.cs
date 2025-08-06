using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.DTOs
{
    public class ImprovementDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
