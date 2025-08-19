using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.ViewModels.SalesType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels.Admin
{
    public class SalesTypeMappingProfile : Profile
    {
        public SalesTypeMappingProfile()
        {
            CreateMap<SaleTypeDto, SalesTypeReadVM>();
            CreateMap<SalesTypeWritteVM, SaleTypeDto>().ReverseMap();
        }
    }
}
