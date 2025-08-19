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

namespace RealStateApp.Core.Application.Features.SaleTypes.Queries
{
    public class GetAllSalesTypeQueryHandler : GetAllListQueryHandler<int, SaleType, SaleTypeDto>
    {
        public GetAllSalesTypeQueryHandler(ISaleTypeRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
