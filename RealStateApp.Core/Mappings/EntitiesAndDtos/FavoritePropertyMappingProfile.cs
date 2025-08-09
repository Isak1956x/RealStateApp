using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos
{
    public class FavoritePropertyMappingProfile : Profile
    {
        public FavoritePropertyMappingProfile()
        {
            CreateMap<FavoriteProperty, FavoritePropertyDto>().ReverseMap();
        }
    }
}
