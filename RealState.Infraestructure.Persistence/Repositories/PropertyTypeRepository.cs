using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using RealState.Infraestructure.Persistence.Context;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealState.Infraestructure.Persistence.Repositories
{
    public class PropertyTypeRepository : BaseRepository<PropertyType, int>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(RealStateContext dbContext) : base(dbContext)
        {
         
        }
    }
}
