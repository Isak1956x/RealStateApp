using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Application.Services
{
    public interface IPropertyService : IGenericService<PropertyDto>
    {
        Task<Result<Unit>> DeletePropertiesOfAgent(string agentId);
        Task<Dictionary<string, int>> GetPropertyCountOfAgents(IEnumerable<string> agentIds);
        Task<PropertyResumeDto> GetResume();
    }
}
