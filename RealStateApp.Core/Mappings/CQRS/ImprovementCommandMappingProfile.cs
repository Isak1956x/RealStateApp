using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Mappings.CQRS
{
    public class ImprovementCommandMappingProfile : CommandsResourceMappingProfile<Improvement, ImprovementDto>
    {
    }
}
