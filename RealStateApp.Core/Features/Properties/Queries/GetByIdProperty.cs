using AutoMapper;
using RealStateApp.Core.Application.Features.Common.GennericQueries;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Properties.Queries
{
    public class GetByIdProperty : GetByIdQueryHandler<int, Domain.Entities.Property, Application.DTOs.PropertyDto>
    {
        public GetByIdProperty(IRepositoryBase<Property, int> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
