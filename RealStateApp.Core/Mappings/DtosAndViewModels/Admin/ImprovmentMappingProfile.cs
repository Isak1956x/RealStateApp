using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.ViewModels.PropertyImprovment;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels.Admin
{
    public class ImprovmentMappingProfile : Profile
    {
        public ImprovmentMappingProfile()
        {
            CreateMap<ImprovementDto, ImprovmentReadVM>();
            CreateMap<ImprovmentWritteVM, ImprovementDto>().ReverseMap();
        }
    }
}
