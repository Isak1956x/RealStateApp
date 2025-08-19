using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GennericQueries;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Queries
{
    public class GetAllImprovements : GetAllListQueryHandler<int, Improvement, ImprovementDto>
    {
        public GetAllImprovements(Domain.Interfaces.IImprovementRepository repository, AutoMapper.IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
