using AutoMapper;
using RealStateApp.Core.Application.Features.Common.GennericQueries;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Queries
{
    public class GetImprovementByIdQueryHandler : GetByIdQueryHandler<int, Domain.Entities.Improvement, Application.DTOs.ImprovementDto>
    {
        public GetImprovementByIdQueryHandler(IRepositoryBase<Improvement, int> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
