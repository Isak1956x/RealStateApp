using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GennericQueries;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Properties.Queries
{
    public class GetAllProperties : GetAllListQueryHandler<int, Property, PropertyDto>
    {
        public GetAllProperties(IRepositoryBase<Property, int> repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}
