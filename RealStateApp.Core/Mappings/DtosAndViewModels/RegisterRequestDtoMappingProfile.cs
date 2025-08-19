using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Core.Application.ViewModels.Login;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels
{
    public class RegisterRequestDtoMappingProfile : Profile
    {
          public RegisterRequestDtoMappingProfile()
        {
            CreateMap<RegisterRequestDTO, RegisterVM>().ReverseMap();
        }
    }
}
