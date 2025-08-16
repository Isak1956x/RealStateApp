using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services
{
    public class SaleTypeService : GenericService<SaleType, SaleTypeDto>, ISaleTypeService
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;
        public SaleTypeService(ISaleTypeRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _saleTypeRepository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SaleTypeDto>> GetAllSalesTypesAsync()
            => await _saleTypeRepository.AsQuery()
                      .Select(st => new SaleTypeDto
                      {
                          Id = st.Id,
                          Name = st.Name,
                          Description = st.Description,
                          PropertyCount = st.Properties.Count
                      }).ToListAsync(); 
    }
}
