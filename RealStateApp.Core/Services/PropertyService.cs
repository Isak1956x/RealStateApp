using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services
{
    public class PropertyService : GenericService<Property, PropertyDto>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        public PropertyService(IPropertyRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _propertyRepository = repository;
            _mapper = mapper;
        }


        public async Task<int> GetPropertyCountOfAgent(string agentId)
        {
            var count = await _propertyRepository.AsQuery()
                                               .Where(p => p.AgentId == agentId)
                                               .CountAsync();
            return count;
        }

        public async Task<Dictionary<string, int>> GetPropertyCountOfAgents(IEnumerable<string> agentIds)
            => await _propertyRepository.AsQuery()
                        .Where(p => agentIds.Contains(p.AgentId))
                        .GroupBy(p => p.AgentId)
                         .Select(g => new {count = g.Count(), id = g.Key})
                        .ToDictionaryAsync(p => p.id, p => p.count);

        public async Task<Result<Unit>> DeletePropertiesOfAgent(string agentId)
        {
            return await _propertyRepository.DeletePropertiesOfAgent(agentId);
        }

        public async Task<PropertyResumeDto> GetResume()
        { 
            var res = await _propertyRepository.AsQuery()
                    .GroupBy(p => p.IsAvailable)
                    .Select(g => new
                    {
                        Active = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();
            return new PropertyResumeDto
            {
                AvailableProperties = res.Where(res => res.Active).Count(),
                SoldProperties = res.Where(res => !res.Active).Count(),
            };
        }
    }
  
}
