using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealState.Infraestructure.Persistence.Repositories
{
    public class SaleTypeRepository : BaseRepository<SaleType, int>, ISaleTypeRepository
    {
        public SaleTypeRepository(RealStateContext dbContext) : base(dbContext)
        {
         
        }

        public override async Task DeleteAsync(int id)
        {
            await _context.Properties
                .Where(p => p.SaleTypeId == id)
                .ExecuteDeleteAsync();
            await base.DeleteAsync(id);
        }
    }
}
