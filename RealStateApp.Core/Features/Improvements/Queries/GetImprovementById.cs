using RealStateApp.Core.Application.Features.Common.GennericQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Queries
{
    public class GetImprovementById : GetByIdQueryHandler<int, Domain.Entities.Improvement, Application.DTOs.ImprovementDto>
    {
        public GetImprovementById(Domain.Interfaces.IImprovementRepository repository, AutoMapper.IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
