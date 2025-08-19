using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands
{
    public class DeletePropertyTypeCommandHandler : DeleteResourceCommandHandler<PropertyType, PropertyTypeDto>
    {
        public DeletePropertyTypeCommandHandler(IPropertyTypeRepository repository) : base(repository)
        {
        }
    }
}
