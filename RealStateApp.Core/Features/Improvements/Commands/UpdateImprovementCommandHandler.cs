using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Improvements.Commands
{
    public class UpdateImprovementCommandHandler : UpdateResourceCommandHandler<Domain.Entities.Improvement, Application.DTOs.ImprovementDto>
    {
        public UpdateImprovementCommandHandler(IRepositoryBase<ImprovementDto, int> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
