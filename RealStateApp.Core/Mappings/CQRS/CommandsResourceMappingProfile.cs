using AutoMapper;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Mappings.CQRS
{
    public abstract class CommandsResourceMappingProfile<TEntity, TDto> : Profile
    {
        public CommandsResourceMappingProfile()
        {
            CreateMap<BaseResourceCommand, TEntity>();
            CreateMap<UpdateResourceCommand<TDto>, TEntity>();

        }
    }
}
