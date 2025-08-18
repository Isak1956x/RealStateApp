using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Agents.Queries
{
    public class GetAllAgentQuery : IRequest<IEnumerable<UserDto>>
    {
    }

    public class GetAllAgentQueryHandler : IRequestHandler<GetAllAgentQuery, IEnumerable<UserDto>>
    {
        private readonly IBaseAccountService _accountService;
        private readonly IMapper _mapper;
        public GetAllAgentQueryHandler(IBaseAccountService agentRepository, IMapper mapper)
        {
            _accountService = agentRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDto>> Handle(GetAllAgentQuery request, CancellationToken cancellationToken)
        {
            return await _accountService.GetByRole(UserRoles.Agent);
            
        }
    }
}
