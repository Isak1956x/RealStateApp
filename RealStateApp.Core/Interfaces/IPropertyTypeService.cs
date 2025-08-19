using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Interfaces;

namespace RealStateApp.Core.Application.Services
{
    public interface IPropertyTypeService : IGenericService<PropertyTypeDto>
    {
        Task<IEnumerable<PropertyTypeDto>> GetAllPropertyTypesAsync();
    }
}
