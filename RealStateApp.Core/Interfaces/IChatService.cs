using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Application.DTOs;

namespace RealStateApp.Core.Application.Interfaces
{
    public interface IChatService : IGenericService<ChatDto>
    {
    }
}
