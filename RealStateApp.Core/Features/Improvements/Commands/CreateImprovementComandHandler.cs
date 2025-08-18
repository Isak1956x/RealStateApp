using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Improvements.Commands
{
    public class CreateImprovementComandHandler : CreateResourceCommandHandler<Domain.Entities.Improvement, ImprovementDto>
    {
        public CreateImprovementComandHandler(IRepositoryBase<Improvement, int> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
