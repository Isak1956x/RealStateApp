using AutoMapper;
using RealStateApp.Core.Application.DTOs;
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
    public class GetPropertyTypeByIdQueryHandler : GetByIdQueryHandler<int, PropertyType, PropertyTypeDto>
    {
        public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
