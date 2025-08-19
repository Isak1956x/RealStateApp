using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands
{
    public class DeleteSaleTypeCommandHandler : DeleteResourceCommandHandler<SaleType, SaleTypeDto>
    {
        public DeleteSaleTypeCommandHandler(ISaleTypeRepository repository) : base(repository)
        {
        }
    }
}
