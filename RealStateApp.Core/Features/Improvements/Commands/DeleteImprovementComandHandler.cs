using RealStateApp.Core.Application.Features.Common.GenericCommands;
using RealStateApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Commands
{
    public class DeleteImprovementComandHandler : DeleteResourceCommandHandler<int, Domain.Entities.Improvement>
    {
        public DeleteImprovementComandHandler(IRepositoryBase<int, int> repository) : base(repository)
        {
        }
    }
}
