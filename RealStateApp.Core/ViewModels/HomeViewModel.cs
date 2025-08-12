using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels
{
    public class HomeViewModel
    {
        public List<PropertyViewModel>? Properties { get; set; }
        public List<PropertyTypeViewModel>? PropertyTypes { get; set; }
        public int? PropertyTypeId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public string? Code { get; set; }
    }
}
