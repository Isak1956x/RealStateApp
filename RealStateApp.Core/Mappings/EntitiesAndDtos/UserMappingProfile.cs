using AutoMapper;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.ViewModels.Login;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, GenericUserReadVM>().ReverseMap();
            CreateMap<GenericUserWritteVM, UserUpdateDTO>()
                .ReverseMap();
            CreateMap<GenericUserWritteEditVM, UserUpdateDTO>()
                .ReverseMap();
            CreateMap<GenericUserWritteVM, RegisterRequestDTO>()
                .ReverseMap();
        }
    }
    
}
