using MediatR;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Agents.Commands
{
    public class ChangueAgentStatusCommand : IRequest<Result<Domain.Base.Unit>>
    {
        public string AgentId { get; set; }
        public bool IsActive { get; set; }
        
    }

    public class ChangeAgentStatusCommandHandler : IRequestHandler<ChangueAgentStatusCommand, Result<Domain.Base.Unit>>
    {
        private readonly IBaseAccountService _accountService;
        public ChangeAgentStatusCommandHandler(IBaseAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Result<Domain.Base.Unit>> Handle(ChangueAgentStatusCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountService.ChangueStatusAgentById(request.AgentId, request.IsActive);
            if (result)
            {
                return Result<Domain.Base.Unit>.Ok(Domain.Base.Unit.Value);
            }
            else
            {
                return Result<Domain.Base.Unit>.Fail("an error occurred while updating agent status");
            }
        }
    }
}
