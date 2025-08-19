using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GennericQueries;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.SaleTypes.Queries
{
    public class GetSalesTypeByIdQueryHandler : GetByIdQueryHandler<int, SaleType, SaleTypeDto>
    {
        public GetSalesTypeByIdQueryHandler(ISaleTypeRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
