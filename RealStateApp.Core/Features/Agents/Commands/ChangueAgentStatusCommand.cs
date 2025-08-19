using MediatR;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Infraestructure.Identity.Service;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Agents.Commands
{
    public class ChangueAgentStatusCommand : IRequest<Result<Domain.Base.Unit>>
    {
        [SwaggerParameter("Unique identifier of agent")]
        public string AgentId { get; set; }
        [SwaggerParameter("Indicates whether the agent is active or not")]
        public bool IsActive { get; set; }
        
    }

    public class ChangeAgentStatusCommandHandler : IRequestHandler<ChangueAgentStatusCommand, Result<Domain.Base.Unit>>
    {
        private readonly IBaseAccountService _accountService;
        public ChangeAgentStatusCommandHandler(IAccountServiceForApi accountService)
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
