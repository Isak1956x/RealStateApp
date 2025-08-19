using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Infraestructure.Identity.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.Agents.Queries
{
    public class GetAgentByIdQuery : IRequest<UserDto>
    {
        [SwaggerParameter("Unique identifier of agent")]
        public string Id { get; set; }
    }
    public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, UserDto>
    {
        private readonly IBaseAccountService _accountService;
        private readonly IMapper _mapper;
        public GetAgentByIdQueryHandler(IAccountServiceForApi accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<UserDto> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
        {
            var agent = await _accountService.GetAgentById(request.Id);
            return _mapper.Map<UserDto>(agent);
        }
    }
}
