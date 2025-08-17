using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealState.Infraestructure.Persistence.Repositories
{
    public class PropertyRepository: BaseRepository<Property, int>, IPropertyRepository
    {
        public PropertyRepository(RealStateContext dbContext) : base(dbContext)
        {
         
        }

        public async Task<Result<Unit>> DeletePropertiesOfAgent(string agentId)
        {
            try
            {
                await _context.Properties.Where(p => p.AgentId == agentId).ExecuteDeleteAsync();
            }
            catch(Exception ex)
            {
                return Result<Unit>.Fail("Something where wrong during delete properties of agent: " + agentId);
            }
            return Unit.Value;
        }
    }
}
