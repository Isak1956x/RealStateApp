using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services
{
    public class PropertyImprovementService : GenericService<PropertyImprovement, PropertyImprovementDto>, IPropertyImprovementService
    {
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly IMapper _mapper;
        public PropertyImprovementService(IPropertyImprovementRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _propertyImprovementRepository = repository;
            _mapper = mapper;
        }
    }
}
