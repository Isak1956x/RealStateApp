using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Mappings.CQRS
{
    public class PropertyTypeCommandsMappingProfile : CommandsResourceMappingProfile<Domain.Entities.PropertyType, PropertyTypeDto>
    {
        
    }
}
