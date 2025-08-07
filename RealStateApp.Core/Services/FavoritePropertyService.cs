using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services
{
    public class FavoritePropertyService : GenericService<FavoriteProperty, FavoritePropertyDto>, IFavoritePropertyService
    {
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;
        private readonly IMapper _mapper;
        public FavoritePropertyService(IFavoritePropertyRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _favoritePropertyRepository =  repository;
            _mapper = mapper;
        }
    }
}
