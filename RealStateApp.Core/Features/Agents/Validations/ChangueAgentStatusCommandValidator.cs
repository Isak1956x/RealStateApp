using FluentValidation;
using RealStateApp.Core.Application.Features.Agents.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Agents.Validations
{
    public class ChangueAgentStatusCommandValidator : AbstractValidator<ChangueAgentStatusCommand>
    {
        public ChangueAgentStatusCommandValidator()
        {
            RuleFor(x => x.AgentId)
                .NotEmpty().WithMessage("Agent ID is required.")
                .NotNull().WithMessage("Agent ID cannot be null.");
            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive status cannot be null.");
        }
    }
}
