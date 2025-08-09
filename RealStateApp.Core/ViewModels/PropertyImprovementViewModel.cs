using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels
{
    public class PropertyImprovementViewModel
    {
        public int PropertyId { get; set; }
        public PropertyViewModel? Property { get; set; }

        public int ImprovementId { get; set; }
        public ImprovementViewModel? Improvement { get; set; }
    }
}
