using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.ViewModels;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels
{
    public class ImprovementDtoMappingProfile : Profile
    {
          public ImprovementDtoMappingProfile()
        {
            CreateMap<ImprovementDto, ImprovementViewModel>().ReverseMap();
        }
    }
}
