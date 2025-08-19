using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RealState.Infraestructure.Persistence.Context;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealState.Infraestructure.Persistence.Repositories
{
    public class PropertyTypeRepository : BaseRepository<PropertyType, int>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(RealStateContext dbContext) : base(dbContext)
        {
         
        }
        public override async Task DeleteAsync(int id)
        {
            await _context.Properties
                .Where(p => p.PropertyTypeId == id)
                .ExecuteDeleteAsync();

            await base.DeleteAsync(id);
        }

        public override async Task<Result<Unit>> DeleteAsync(PropertyType entity)
        {
            await _context.Properties
                .Where(p => p.PropertyTypeId == entity.Id)
                .ExecuteDeleteAsync();
            return await base.DeleteAsync(entity);
        }


    }
}
