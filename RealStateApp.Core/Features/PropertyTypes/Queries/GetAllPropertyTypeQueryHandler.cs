using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Agents.Queries;
using RealStateApp.Core.Application.Features.Common.GennericQueries;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Queries
{
    public class GetAllPropertyTypeQueryHandler : GetAllListQueryHandler<int, PropertyType, PropertyTypeDto>
    {
        public GetAllPropertyTypeQueryHandler(IPropertyTypeRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
