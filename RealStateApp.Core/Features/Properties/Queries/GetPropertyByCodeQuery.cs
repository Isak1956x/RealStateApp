using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Properties.Queries
{
    public class GetPropertyByCodeQuery : IRequest<PropertyDto>
    {
        public string Code { get; set; }
        
    }

    public class GetPropertyByCodeQueryHandler : IRequestHandler<GetPropertyByCodeQuery, PropertyDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        public GetPropertyByCodeQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }
        public async Task<PropertyDto> Handle(GetPropertyByCodeQuery request, CancellationToken cancellationToken)
        {
            var entity = await _propertyRepository.GetByCode(request.Code);
            return _mapper.Map<PropertyDto>(entity);
        }
    }
}
