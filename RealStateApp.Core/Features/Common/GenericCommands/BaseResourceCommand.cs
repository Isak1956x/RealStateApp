using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Common.GenericCommands
{
    public class BaseResourceCommand
    {
        // public int Id { get; set; }
        [SwaggerParameter("Name of th Resource")]
        public string Name { get; set; }
        [SwaggerParameter("Description of th Resource")]
        public string Description { get; set; }
    }
}
