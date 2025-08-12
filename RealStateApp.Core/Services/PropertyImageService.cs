using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services
{
    public class PropertyImageService : GenericService<PropertyImage, PropertyImageDto>, IPropertyImageService
    {
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IMapper _mapper;
        public PropertyImageService(IPropertyImageRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _propertyImageRepository = repository;
            _mapper = mapper;
        }
    }
  
}
