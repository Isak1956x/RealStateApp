using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services
{
    public class PropertyTypeService : GenericService<PropertyType, PropertyTypeDto>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;
        public PropertyTypeService(IPropertyTypeRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _propertyTypeRepository =  repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyTypeDto>> GetAllPropertyTypesAsync()
            => await _propertyTypeRepository.AsQuery()
            .Select(pt => new PropertyTypeDto
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description,
                PropertyCount = pt.Properties.Count
            }).ToListAsync();
    }
}
