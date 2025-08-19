using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Agents.Queries
{
    public class GetAgentsPropertiesQuery : IRequest<IEnumerable<PropertyDto>>
    {
        [SwaggerParameter("Unique identifier of agent")]
        public string AgentId { get; set; }
        
    }

    public class GetAgentsPropertiesQueryHandler : IRequestHandler<GetAgentsPropertiesQuery, IEnumerable<PropertyDto>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        public GetAgentsPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PropertyDto>> Handle(GetAgentsPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = await _propertyRepository.GetPropertiesByAgentId(request.AgentId);
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }
    }
}
