using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services
{
    public class ImprovementService : GenericService<Improvement, ImprovementDto>, IImprovementService
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;
        public ImprovementService(IImprovementRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _improvementRepository = repository;
            _mapper = mapper;
        }


    }
}
