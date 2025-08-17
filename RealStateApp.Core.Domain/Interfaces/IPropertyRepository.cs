using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Domain.Interfaces
{
    public interface IPropertyRepository : IRepositoryBase<Property, int>
    {
        Task<Result<Unit>> DeletePropertiesOfAgent(string agentId);
    }
}
