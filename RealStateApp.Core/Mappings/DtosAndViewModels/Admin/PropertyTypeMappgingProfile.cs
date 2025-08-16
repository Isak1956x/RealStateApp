using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.ViewModels.PropertyTypes;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels.Admin
{
    public class PropertyTypeMappgingProfile : Profile
    {
        public PropertyTypeMappgingProfile()
        {
            CreateMap<PropertyTypeDto, PropertyTypeReadVM>();
            CreateMap<PropertyTypeWriteVM, PropertyTypeDto>();
        }
    }
}
