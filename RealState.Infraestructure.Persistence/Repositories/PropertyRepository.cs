using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealState.Infraestructure.Persistence.Context;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealState.Infraestructure.Persistence.Repositories
{
    public class PropertyRepository: BaseRepository<Property, int>, IPropertyRepository
    {
        public PropertyRepository(RealStateContext dbContext) : base(dbContext)
        {
         
        }
    }
}
