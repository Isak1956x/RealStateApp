using AutoMapper;
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
    public class UpdateSalesTypeCommandHandler : UpdateResourceCommandHandler<SaleTypeDto, SaleType>
    {
        public UpdateSalesTypeCommandHandler(IRepositoryBase<SaleType, int> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
